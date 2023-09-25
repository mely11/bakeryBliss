using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    Debug.Log("rb assigned: " + (rb != null));
    //}

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LadderMovement : MonoBehaviour
//{
//    private float vertical;
//    private float speed = 8f;
//    private bool isLadder; //a boolean that tells whether the player is standing next to the ladder
//    private bool isClimbing;//a boolean that tells whether the player is climbing
//    [SerializeField] private Rigidbody2D rb;//serialized field to reference to the player rigidbody2d

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        vertical = Input.GetAxis("vertical");
//        if (isLadder && Mathf.Abs(vertical) > 0f)
//        {
//            isClimbing = true;
//        }
//    }
//    //disable gravity and move our player
//    private void FixedUpdate()
//    {
//        if (isClimbing)
//        {
//            rb.gravityScale = 0f;
//            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);

//        }
//        else
//        {
//            rb.gravityScale = 4f;// if it is not climbing, set the gravity as it is
//        }
//    }
//    //to check if the player is standing next to the ladder

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Ladder"))
//        {
//            //set isLadder to true when we enter the trigger
//            isLadder = true;
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Ladder"))
//        {
//            //set isladder to false when we exit the trigger
//            isLadder = false;
//            isClimbing = false;
//        }
//    }

//}

