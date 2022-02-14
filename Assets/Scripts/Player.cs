using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movePower;
    public float jumpPower;
    

    
    private float rayHitDistance = 0.0f;
    Rigidbody2D rigid;
    private Animator animator;

    
    private SpriteRenderer spriterenderer;
    
    
    Vector3 movement;
    bool isJumping = false;

    //원거리 공격 변수
    public GameObject bullet;
    public Transform bulletPos;
    
    
    void Start ()
    {
        rigid = gameObject.GetComponent<Rigidbody2D> ();
        //rigidPosition = new Vector2(rigid.position.x, rigid.position.y-);
        spriterenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    //기본공격 쿨타임 변수
    private float normalCurTime;
    public float normalCoolTime = 0.4f;

    //원거리 공격 쿨타임 변수
    private float bulletCurTime;
    public float bulletCoolTime;
    
    
    public Transform pos;
    public Vector2 boxSize;
    
    
    
    
    
    void Update ()
    {
        
        //점프키 입력 받는 코드
        if (Input.GetKeyDown(KeyCode.Z)==true && rayHitDistance>=0.010f) 
            isJumping = true;

        
        
        //기본 공격
        normalAttack();
        
        //원거리 공격
        bulletAttack();
        
    }
    

    //Physics engine Updates
    void FixedUpdate ()
    {
        Moving ();
        Jump ();
        fallingCheck();
    }

    

    void Moving ()
    {		
        Vector3 moveVelocity= Vector3.zero;

        if (Input.GetAxisRaw ("Horizontal") < 0) {
            moveVelocity = Vector3.left;
            animator.SetBool("run", true);
            spriterenderer.flipX = true;
            

        }
			
        else if(Input.GetAxisRaw ("Horizontal") > 0){
            moveVelocity = Vector3.right; 
            animator.SetBool("run", true);
            spriterenderer.flipX = false;
        }
        else
        {
            animator.SetBool("run", false);
        }
        
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump ()
    {
        Debug.DrawRay(rigid.position,Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1,LayerMask.GetMask("Floor"));
        // Debug.Log("RaycastHit Distance :"+rayHit.distance);
        rayHitDistance = rayHit.distance;
        
        if (!isJumping || rayHit.collider == null || rayHit.distance>=0.023f)
            return;
        
        //Prevent Velocity amplification.
        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2 (0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        animator.SetBool("isJumping", true);
        
        //Debug.Log("점프하고 있는가?: "+isJumping);
        isJumping = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            onDamaged(col.transform.position);
        }
    }

    void onDamaged(Vector2 targetPos)
    {
        gameObject.layer = 8;
        
        spriterenderer.color = new Color(1, 1, 1, 0.4f);

        int dirc = targetPos.x - transform.position.x > 0 ? -1 : 1;
        rigid.AddForce(new Vector2(dirc,1) * 7, ForceMode2D.Impulse);
        
        Invoke("offDamaged", 2);
    }

    void offDamaged()
    {
        gameObject.layer = 7;
        spriterenderer.color = new Color(1, 1, 1, 1);
    }

    void fallingCheck()
    {
        if (rigid.velocity.y == 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (rigid.velocity.y < 0 && !animator.GetBool("isJumping"))
            animator.SetBool("isFalling", true);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
            Gizmos.DrawWireCube(pos.position, boxSize);
            Gizmos.DrawWireCube(new Vector3(pos.position.x - 2.2f, pos.position.y, pos.position.z), boxSize);
    }

    void normalAttack()
    {
        
        if (normalCurTime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                Collider2D[] collider2Ds;
                if(!spriterenderer.flipX)
                    collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                else 
                    collider2Ds = Physics2D.OverlapBoxAll(new Vector2(pos.position.x - 2.2f, pos.position.y), boxSize, 0);
        
                
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().onDamaged(1, transform.position);
                        collider.GetComponent<Enemy>().beingDamaged = true;
                        StartCoroutine(beingDamagedFalse());

                    }
                }
                
                
                animator.SetTrigger("attack");
                normalCurTime = normalCoolTime;
            }
        }
        else
        {
            normalCurTime -= Time.deltaTime;
        }
    }

    void bulletAttack()
    {
        if (bulletCurTime <= 0)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Instantiate(bullet, bulletPos.position, transform.rotation);
                bulletCurTime = bulletCoolTime;
                animator.SetTrigger("attack");
            }
        }
        else
        {
            bulletCurTime -= Time.deltaTime;
        }
    }

    IEnumerator beingDamagedFalse()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Enemy").GetComponent<Enemy>().beingDamaged = false; 
    }
    
}