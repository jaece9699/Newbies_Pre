using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy _enemy;
    
    Rigidbody2D rigid;
    public int nextMove;

    public int Hp;
    public int currentHp;
    
    public bool beingDamaged = false;
    
    public int atk; //공격력
    public int def; //방어력
    
    void Awake()
    {
        _enemy = this;
        rigid = GetComponent<Rigidbody2D>();
        Think();
    }
    
    
    
    void FixedUpdate()
    {
        
        //몬스터가 피격 되었을 때 뒤로 살짝 밀리게 하기 위해서 움직임을 잠깐 멈추는 코드임.
        //Player 스크립트에서 코루틴으로 beingDamaged 변수의 값을 조정함.
        if (!beingDamaged)
            move();
        
        



    }

    void move()
    {
        rigid.velocity = new Vector2(nextMove * 4, rigid.velocity.y);
        Vector2 fontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(fontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(fontVec, Vector2.down, 1, LayerMask.GetMask("Floor"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", Random.Range(1f, 4f));

        }
    }

    
    
    void Think()
    {
        nextMove = Random.Range(-1, 2);
        float nextThinkTime = Random.Range(2f, 5f);
        
        Invoke("Think", nextThinkTime);
    }

    public void onDamaged(int damage, Vector2 targetPos)
    {
        PlayerStat._playerStat.atk = damage;
        // Hp = Hp - damage;
        int dirc = targetPos.x - transform.position.x > 0 ? -1 : 1;
        Debug.Log(dirc);
        rigid.velocity = new Vector2(dirc*4, 3);
    }
}
