using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Player : MonoBehaviour
{
    public static Player _player;

    public float movePower;
    public float jumpPower;
    

    
    private float rayHitDistance = 0.0f;
    Rigidbody2D rigid;
    private Animator animator;

    
    private SpriteRenderer spriterenderer;
    
    
    Vector3 movement;
    bool isJumping = false;
    bool isJumpingSound = false;

    //원거리 공격 변수
    public GameObject bullet;
    public Transform bulletPos;

    private bool isPlayerDead = false; //죽음 판단

    private PlayerStat _playerStat;
    private EnemyStat _enemyStat;
    private MushroomStat _mushroomStat;
    private GoblinStat _goblinStat;
    private GhostStat _ghostStat;



    public string runningSound;
    public string jumpSound;
    private AudioManager TheAudio;
    
    
    void Start ()
    {
        
        rigid = gameObject.GetComponent<Rigidbody2D> ();
        //rigidPosition = new Vector2(rigid.position.x, rigid.position.y-);
        spriterenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _playerStat = GetComponent<PlayerStat>();
        TheAudio = FindObjectOfType<AudioManager>();
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
        if(isPlayerDead){return;} //죽었다면 조작하지 못하도록 
        
        //점프키 입력 받는 코드
        if (Input.GetKeyDown(KeyCode.Z)==true && rayHitDistance>=0.010f) 
            isJumping = true;

        

        
        
        //기본 공격
        normalAttack();
        
        //원거리 공격
        bulletAttack();
        
        //회복 스킬
        PlayerStat._playerStat.healSkill(); 

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
            if(isJumpingSound==false)
                TheAudio.Play(runningSound);
            else
                isJumpingSound = false;
            spriterenderer.flipX = true;
            

        }
			
        else if(Input.GetAxisRaw ("Horizontal") > 0){
            moveVelocity = Vector3.right; 
            animator.SetBool("run", true);
            if(isJumpingSound==false)
                TheAudio.Play(runningSound);
            else
                isJumpingSound = false;
            spriterenderer.flipX = false;
        }
        else
        {
            animator.SetBool("run", false);
            TheAudio.Stop(runningSound);
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
            isJumpingSound = true;
            isJumping = false;
            TheAudio.Play(jumpSound);
            
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            string monsterName = col.gameObject.name.Substring(0, 3);
            switch (monsterName)
            {
                case "Sli":
                    _enemyStat = col.gameObject.GetComponent<EnemyStat>();
                    PlayerStat._playerStat.Hit(_enemyStat.atk);
                    break;
                            
                case "Mus":
                    _mushroomStat = col.gameObject.GetComponent<MushroomStat>();
                    PlayerStat._playerStat.Hit(_mushroomStat.atk);
                    break;
                
                case "Gob":
                    _goblinStat = col.gameObject.GetComponent<GoblinStat>();
                    PlayerStat._playerStat.Hit(_goblinStat.atk);
                    break;
                
                case "Gho":
                    _ghostStat = col.gameObject.GetComponent<GhostStat>();
                    PlayerStat._playerStat.Hit(_ghostStat.atk);
                    break;
            }
            
            onDamaged(col.transform.position);
            
        }
    }

    public void onDamaged(Vector2 targetPos)
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
                        string monsterName = collider.name.Substring(0, 3);
                        switch (monsterName)
                        {
                            case "Sli":
                                collider.GetComponent<Enemy>().onDamaged(transform.position);
                                collider.GetComponent<Enemy>().beingDamaged = true;
                                break;
                            
                            case "Mus":
                                collider.GetComponent<Mushroom>().onDamaged(transform.position);
                                collider.GetComponent<Mushroom>().beingDamaged = true;
                                break;
                            
                            case "Gob":
                                collider.GetComponent<Goblin>().onDamaged(transform.position);
                                collider.GetComponent<Goblin>().beingDamaged = true;
                                break;
                            
                            case "Gho":
                                collider.GetComponent<Ghost>().onDamaged(transform.position);
                                collider.GetComponent<Ghost>().beingDamaged = true;
                                break;
                            
                        }
                        StartCoroutine(beingDamagedFalse(collider));
                    }
                }
                
                animator.SetTrigger("attack");
                normalCurTime = normalCoolTime;
            }
            //회복 스킬
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (PlayerStat._playerStat.currentHp >= 5) 
                    return;
                
                normalCurTime = 10f;
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
            if (Input.GetKeyDown(KeyCode.A))
            {
                Instantiate(bullet, bulletPos.position, transform.rotation);
                bulletCurTime = bulletCoolTime;
                animator.SetTrigger("attack");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (PlayerStat._playerStat.currentHp >= 5)
                    return;
                
                bulletCurTime = 10f;
            }
        }
        else
        {
            bulletCurTime -= Time.deltaTime;
        }
    }

    IEnumerator beingDamagedFalse(Collider2D col)
    {
        yield return new WaitForSeconds(0.5f);
        string monsterName = col.name.Substring(0, 3);
        switch (monsterName)
        {
            case "Sli":
                col.GetComponent<Enemy>().beingDamaged = false;
                break;
                            
            case "Mus":
                col.GetComponent<Mushroom>().beingDamaged = false;
                break;
        }
    }
    
    IEnumerator CheckPlayerDeath()
    {
        while(true)
        {
            // 땅 밑으로 떨어졌다면
            if (this.transform.position.y < -8)
            {
                
            }
            
            // 체력이 0이하일 때
            if (PlayerStat._playerStat.currentHp <= 0)
            {
                isPlayerDead = true;
                // animator.SetTrigger("die");
                yield return new WaitForSeconds(2); // 2초 기다리기
                // SceneManager.LoadScene("Main");
            }
            yield return new WaitForEndOfFrame(); // 매 프레임의 마지막 마다 실행
        }
    }
    
    // 즉시 모든 활동을 끝내고(움직임, 상호작용 등등의 강제 정지) 연출
    // 변수들을 초기화하고, 필요한 프리펩만 새로 생성하는 함수를 만들어서 로딩을 최소한으로 줄이는 게 좋을 것 같습니다.
 
}