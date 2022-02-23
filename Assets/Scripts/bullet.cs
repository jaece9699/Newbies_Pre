using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    private SpriteRenderer playerSprite;
    private bool isFlip;
    private BoxCollider2D col;
    private SpriteRenderer spriterenderer;
    

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        spriterenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(realDestroyBullet());
        Invoke("fakeDestroyBullet", 0.7f);
        
        playerSprite = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        isFlip = playerSprite.flipX;
    }

    
    void Update()
    {
        if(!isFlip)
            transform.Translate(transform.right * speed * Time.deltaTime);
        else
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);

    }

    void fakeDestroyBullet()
    {
        col.enabled = false;
        spriterenderer.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().onDamaged(transform.position);
            
            collision.gameObject.GetComponent<Enemy>().beingDamaged = true;
            StartCoroutine(beingDamagedFalse(collision.gameObject.GetComponent<Enemy>()));
            fakeDestroyBullet();
            
        }
        else if(collision.gameObject.tag == "Floor")
            Destroy(gameObject);

    }
    
    IEnumerator beingDamagedFalse(Enemy enemy)
    {
        
        yield return new WaitForSeconds(0.5f);
        enemy.beingDamaged = false; 
        
    }
    
    IEnumerator realDestroyBullet()
    {
        yield return new WaitForSeconds(3);
            Destroy(gameObject);
    }
}