using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rigid;
    private bool isDash = false;
    private SpriteRenderer spriterenderer;
    public float dashSpeed;
    public float dashDuration;

    private bool isPossible = true;
    public float coolTime;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C) && isPossible)
        {
            isDash = true;
            isPossible = false;
            StartCoroutine(isDashing());
            StartCoroutine(dashCooltime());
        }

        if (isDash)
        {
            if (!spriterenderer.flipX)
                rigid.velocity = new Vector2(dashSpeed, rigid.velocity.y);
            else
                rigid.velocity = new Vector2(dashSpeed * -1 , rigid.velocity.y);
                
        }
        
        
    }
    
    IEnumerator isDashing()
    {
        yield return new WaitForSeconds(dashDuration);
        isDash = false;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }
    
    IEnumerator dashCooltime()
    {
        yield return new WaitForSeconds(coolTime);
        isPossible = true;
    }

    
    
}
