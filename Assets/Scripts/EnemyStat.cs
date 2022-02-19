using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public static EnemyStat _enemyStat;

    public int atk; //공격력
    public int def; //방어력
    public int currentHp;
    public int hp;

    void Start()
    {
        currentHp = hp;
        _enemyStat = this;
    }

    public void Hit(int playerAtk)
    {
        int dmg = playerAtk;
        currentHp -= dmg;

        if (currentHp <= 0)
        {
            //에니메이션,이펙트 추가
            //Destroy(gameObject);
        }
    }

}
