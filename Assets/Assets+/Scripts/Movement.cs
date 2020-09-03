using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;

    [Space]

    [Header("Parkour")]
    public float slideSpeed;
    public float wallJumpLerp;
    public float wallJumpForce;

    [Space]

    [Header("Booleans")]
    public bool wallJumped;
    public bool canMove;

    [Space]

    [Header("Other")]
    public int side = 1;


    private Rigidbody2D rb;
    CollisionState coll;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CollisionState>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal"); //Project Settings Defined
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);

        if ((Input.GetKeyDown(KeyCode.Space)))
        {
            if (coll.onWall && !coll.onGround)
            {
                WallJump();
            }
            else if (coll.onGround && coll.onWall)
            {
                Jump(Vector2.up, false);
                StartCoroutine(DisableWallJump(1f));
                StopCoroutine(DisableWallJump(0f));
            }
            else if (coll.onGround)
            {
                Jump(Vector2.up, false);
            }
        }

        Vector3 curVelocity = rb.velocity;

        if (Input.GetKey(KeyCode.K))
        {
            Debug.Log(curVelocity);
        }

        if (coll.onGround == true)
        {
            wallJumped = false;
        }

        if(coll.onWall == true)
        {
            WallSlide();
        }

        //Flip();
    }


    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;
        if (!wallJumped)
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        else
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
    }

    private void WallJump() //Wall jump
    {
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        if (coll.onRightWall == true)
        {
            jumpWall((Vector2.left / 3f + wallDir / 1.75f), true);
        }
        else if (coll.onLeftWall == true)
        {
            jumpWall((Vector2.right / 3f + wallDir / 1.75f), true);
        }
        jumpWall((Vector2.up / 1.5f + wallDir / 1.5f), true);
        StopCoroutine(DisableMovement(0f));
    }

    IEnumerator DisableMovement(float time) //For a small period of time after a wall jump
    {
        canMove = false;
        wallJumped = true;
        yield return new WaitForSeconds(time);
        canMove = true;
        wallJumped = false;
    }

    IEnumerator DisableWallJump(float time) //For a small period of time after a wall jump.
    {
        wallJumped = true;
        yield return new WaitForSeconds(time);
        wallJumped = false;
    }

    private void Jump(Vector2 dir, bool wall) //Using Jump.cs
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }

    private void jumpWall(Vector2 dir, bool wall) //Wall Jump
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * wallJumpForce;
    }

    private void WallSlide() //Wall Slide
    {
        if(wallJumped == false)
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);

        //if (!canMove)
           // return;
    }
    //Being saved for when sprites get introduced.
    /*void Flip() //Flips Sprite
    {
        if(coll.onWall == false && canMove == true)
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
            
            }
        if(coll.onRightWall == true && coll.onGround == false && canMove == true)
            GetComponent<SpriteRenderer>().flipX = true;
        else if(coll.onLeftWall == true && coll.onGround == false && canMove == true)
            GetComponent<SpriteRenderer>().flipX = false;
    }*/
}
