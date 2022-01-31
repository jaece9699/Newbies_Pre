using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float movePower = 10f;
    public float jumpPower = 10f;

    private float rayHitDistance = 0.0f;
    Rigidbody2D rigid;
    //Vector2 rigidPosition;
    
    Vector3 movement;
    bool isJumping = false;
    //---------------------------------------------------[Override Function]
    //Initialization
    void Start ()
    {
        rigid = gameObject.GetComponent<Rigidbody2D> ();
        //rigidPosition = new Vector2(rigid.position.x, rigid.position.y-);
    }

    //Graphic & Input Updates
    void Update ()
    {
        if (Input.GetButtonDown ("Jump")==true && rayHitDistance>=1.4f) {
            isJumping = true;
        }
    }

    //Physics engine Updates
    void FixedUpdate ()
    {
        Moving ();
        Jump ();
    }

    //---------------------------------------------------[Movement Function]

    void Moving ()
    {		
        Vector3 moveVelocity= Vector3.zero;

        if (Input.GetAxisRaw ("Horizontal") < 0) {
            moveVelocity = Vector3.left;
        }
			
        else if(Input.GetAxisRaw ("Horizontal") > 0){
            moveVelocity = Vector3.right;
        }	

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump ()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 2,LayerMask.GetMask("Floor"));
        Debug.Log("RaycastHit Distance :"+rayHit.distance);
        rayHitDistance = rayHit.distance;
        
        if (!isJumping || rayHit.collider == null || rayHit.distance>=1.55f)
            return;
        
        //Prevent Velocity amplification.
        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2 (0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        //Debug.Log("점프하고 있는가?: "+isJumping);
        isJumping = false;
    }
}