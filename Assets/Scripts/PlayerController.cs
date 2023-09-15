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
    public float jumpPower = 8.0f;
    public bool isGrounded = true;
    
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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        // -------------- horizontal mechanics --------------
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);

        // -------------- jump mechanics -------------
        if (isGrounded)
        {
            // only allow the player to jump when they are in contact with the ground
            jumpInput = Input.GetAxis("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpPower * jumpInput);
        }

        if (rb.velocity.y < 0)
        {
            // if player has started falling down, then increase its speed
            rb.velocity -= gravity * Time.deltaTime * Math.Abs(transform.position.y - currentGround.transform.position.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Floor"))
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
}
