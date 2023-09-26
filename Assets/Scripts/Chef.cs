using UnityEngine;

[RequireComponent(typeof(ChefController2D))]
public class Chef : MonoBehaviour
{
    // Jump parameters.
    public float maxJumpHeight = 5.5f, minJumpHeight = 1.1f, timeToJumpApex = .49f;
    private readonly float accelerationTimeAirborne = .21f;
    private readonly float accelerationTimeGrounded = .11f;

    // Movement speed and wall jump settings.
    private readonly float moveSpeed = 8.0f;
    public Vector2 wallJumpClimb, wallJumpOff, wallLeap;

    // Wall sliding settings.
    public float wallSlideSpeedMax = 3.1f, wallStickTime = .23f;
    private float timeToWallUnstick, gravity, maxJumpVelocity, minJumpVelocity, velocityXSmoothing;
    private Vector3 velocity;
    private ChefController2D controller;
    private Vector2 directionalInput;
    private bool slideWall;
    private int wallDirX;

    private void Start()
    {
        controller = GetComponent<ChefController2D>();
        CalculateJumpParameters(); // Initialize jump-related constants.
    }

    private void CalculateJumpParameters()
    {
        // Calculate gravity and jump velocities.
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    private void Update()
    {
        CalculateVelocity();
        HandleWallSliding();
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Move the chef based on calculated velocity.
        controller.Move(velocity * Time.deltaTime, directionalInput);

        // Handle collisions and sliding down slopes.
        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                // Adjust the velocity when sliding down slopes.
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input) => directionalInput = input;

    public void OnJumpDown()
    {
        if (slideWall)
        {
            // Handle wall jump logic.
            HandleWallJ();
        }
        else if (controller.collisions.below)
        {
            // Handle regular jumps.
            HandleRegularJ();
        }
    }

    private void HandleWallJ()
    {
        if (wallDirX == directionalInput.x)
        {
            SetVelocity(-wallDirX * wallJumpClimb.x, wallJumpClimb.y);
        }
        else if (directionalInput.x == 0)
        {
            SetVelocity(-wallDirX * wallJumpOff.x, wallJumpOff.y);
        }
        else
        {
            SetVelocity(-wallDirX * wallLeap.x, wallLeap.y);
        }
    }

    private void HandleRegularJ()
    {
        if (controller.collisions.slidingDownMaxSlope)
        {
            if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
            { // Not jumping against max slope.
                SetVelocity(maxJumpVelocity * controller.collisions.slopeNormal.x, maxJumpVelocity * controller.collisions.slopeNormal.y);
            }
        }
        else
        {
            SetVelocity(velocity.x, maxJumpVelocity);
        }
    }

    private void SetVelocity(float x, float y)
    {
        velocity.x = x;
        velocity.y = y;
    }

    public void OnJumpUp()
    {
        // Handle releasing the jump button.
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        slideWall = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            slideWall = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void CalculateVelocity()
    {
        // Calculate the chef's movement velocity.
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}