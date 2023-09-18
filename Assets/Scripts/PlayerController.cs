using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // keyboard inputs that can affect player movement
    public float horizontalInput;
    public float verticalInput;
    public float jumpInput;
    
    // properties
    public float speed = 5.0f;
    public float maxSpeedMultiplier = 1.0f;
    public float jumpPower = 10.0f;
    public bool isGrounded = true;
    public bool isAscending;
    public bool isDescending;
    private float maxFallVelocity = 40.0f;
    
    // Components
    private Vector2 gravity;
    private Rigidbody2D rb;
    private GameObject currentGround;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity = new Vector2(0, -Physics2D.gravity.y);
        currentGround = GameObject.Find("Floor");
    }

    void Update()
    {
        setAscendingDescending();
        
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // -------------- horizontal mechanics --------------
        //transform.Translate(Vector2.right * Time.deltaTime * horizontalInput * speed);
        if (Math.Abs(horizontalInput) == 1.0f)
        {
            if (maxSpeedMultiplier < 2.0f)
            {
                maxSpeedMultiplier += 0.1f;
            }
        }
        else
        {
            maxSpeedMultiplier = 1.0f;
        }
        rb.velocity = new Vector2(horizontalInput * speed * maxSpeedMultiplier, rb.velocity.y);
        

        // -------------- jump mechanics -------------
        if (isGrounded)
        {
            // only allow the player to jump when they are in contact with the ground
            jumpInput = Input.GetAxis("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpPower * jumpInput);
        }

        // slow down the jump velocity once they release the jump button
        if (Input.GetButtonUp("Jump") && !isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        if (rb.velocity.y < 0)
        {
            // if player has started falling down, then increase its speed
            rb.velocity -= gravity 
                           * Time.deltaTime 
                           * Math.Max(Math.Abs(transform.position.y - currentGround.transform.position.y), -maxFallVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Floor") && (!isAscending || !isDescending))
        {
            isGrounded = true;
            currentGround = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Floor"))
        {
            isGrounded = false;
        }
    }

    private void setAscendingDescending()
    {
        if (rb.velocity.y > 0)
        {
            isAscending = true;
            isDescending = false;
        }
        else if (rb.velocity.y < 0)
        {
            isDescending = true;
            isAscending = false;
        }
        else
        {
            isDescending = false;
            isAscending = false;
        }
    }
}
