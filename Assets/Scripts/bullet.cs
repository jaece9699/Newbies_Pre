using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    private SpriteRenderer playerSprite;
    private bool isFlip;
    void Start()
    {
        Invoke("destroyBullet", 1);
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

    void destroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().onDamaged(1, transform.position);
            collision.gameObject.GetComponent<Enemy>().beingDamaged = true;
            StartCoroutine(beingDamagedFalse());
            Destroy(gameObject);

        }
        else if(collision.gameObject.tag == "Floor")
            Destroy(gameObject);
    }
    
    IEnumerator beingDamagedFalse()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Enemy").GetComponent<Enemy>().beingDamaged = false; 
    }
}

