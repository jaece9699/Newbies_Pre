using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float movePower = 10f;
    public float jumpPower = 10f;

    private float rayHitDistance = 0.0f;
    Rigidbody2D rigid;
    private Animator animator;

    private SpriteRenderer spriterenderer;
    //Vector2 rigidPosition;
    
    Vector3 movement;
    bool isJumping = false;
    //---------------------------------------------------[Override Function]
    //Initialization
    void Start ()
    {
        rigid = gameObject.GetComponent<Rigidbody2D> ();
        //rigidPosition = new Vector2(rigid.position.x, rigid.position.y-);
        spriterenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private float curTime;
    private float coolTime = 0.4f;
    
    //Graphic & Input Updates
    void Update ()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Z)==true && rayHitDistance>=0.010f) 
            isJumping = true;

        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetTrigger("attack");
                curTime = coolTime;

            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    //Physics engine Updates
    void FixedUpdate ()
    {
        Moving ();
        Jump ();
        fallingCheck();
       
        
    }

    //---------------------------------------------------[Movement Function]

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
        rigid.AddForce(new Vector2(dirc,1) * 7,ForceMode2D.Impulse);
        
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
}