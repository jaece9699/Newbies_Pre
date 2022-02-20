using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 체력 = 5칸  피격 시: 체력 -1 ~ -2
// 체력 채우기: 스킬 -사용하면 즉시 체력 1칸 회복에 10초 공격 불가 및 쿨타임  -휴식(장소

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat _playerStat;
    
    public int hp; //hp
    public int currentHp; //현재 hp
    public int atk; //공격력
    public int def; //방어력
    
    public Slider hpSilder;
    
    private float healCurTime;
    public float healCoolTime = 10f;
    
    
    void Start()
    {
        _playerStat = this;
        currentHp = hp;
    }

    public void Hit(int enemyAtk) //맞을 때 hp 줄어듦
    {
        int dmg = enemyAtk;

        currentHp -= dmg;

        if (currentHp <=0)
            Debug.Log("게임오버");
    }

    public void healSkill()
    {
        if(healCurTime <= 0)
        {
            if(Input.GetKey(KeyCode.S)) 
            {
                if (currentHp >= 5)
                {
                    Debug.Log("체력이 가득 차있습니다.");
                    return;
                }

                currentHp += 1; 
                healCurTime = healCoolTime;
            }
        }
        else
            healCurTime -= Time.deltaTime;
    }

    void Update()
    {
        hpSilder.maxValue = hp;
        hpSilder.value = currentHp;
    }
}