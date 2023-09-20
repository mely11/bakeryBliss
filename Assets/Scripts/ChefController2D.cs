using UnityEngine;

public class ChefController2D : RaycastController
{
    // Maximum slope angle the chef can handle
    public float maxSlopeAngle = 80;

    // Information about collisions
    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;

    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
    }

    public void Move(Vector2 moveAmt, bool onPlatform) => Move(moveAmt, Vector2.zero, onPlatform);

    public void Move(Vector2 moveAmt, Vector2 chefInput, bool onPlatform = false)
    {
        // Update the raycast origins based on the chef's position
        UpdateRaycastOrigins();

        // Reset collision information for this frame
        collisions.Reset();
        collisions.moveAmtOld = moveAmt; // Store the old move amount for later reference
        playerInput = chefInput; // Store user input

        if (moveAmt.y < 0)
        {
            // Handle descending slopes
            DescendSlope(ref moveAmt);
        }

        if (moveAmt.x != 0)
        {
            // Determine the direction the chef is facing
            collisions.faceDir = (int)Mathf.Sign(moveAmt.x);
        }

        // Check for horizontal collisions
        HorizontalCollisions(ref moveAmt);

        // Check for vertical collisions
        if (moveAmt.y != 0)
        {
            VerticalCollisions(ref moveAmt);
        }

        // Move the chef based on the calculated move amount
        transform.Translate(moveAmt);

        if (onPlatform)
        {
            // The chef is standing on a platform
            collisions.below = true;
        }
    }

    private void HorizontalCollisions(ref Vector2 moveAmt)
    {
        float dirX = collisions.faceDir, rayLen = Mathf.Abs(moveAmt.x) + skinWidth;

        if (Mathf.Abs(moveAmt.x) < skinWidth)
        {
            // Ensure a minimum ray length to avoid issues
            rayLen = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            // Cast a ray to check for collisions
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLen, collisionMask);

            // Debug visualization of raycasts
            Debug.DrawRay(rayOrigin, Vector2.right * dirX, Color.red);

            if (hit && hit.distance != 0)
            {
                // Calculate the slope angle of the surface
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle <= maxSlopeAngle) ClimbSlope(ref moveAmt, slopeAngle, hit.normal);

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    // Handle non-slope collisions
                    moveAmt.x = (hit.distance - skinWidth) * dirX;
                    rayLen = hit.distance;
                    // Update collision flags
                    collisions.left = dirX == -1;
                    collisions.right = dirX == 1;
                }
            }

            //if (hit.distance == 0)
            //{
            //    // Continue to the next ray if the hit point is at the origin
            //    continue;
            //}
            //    if (i == 0 && slopeAngle <= maxSlopeAngle)
            //    {
            //        if (collisions.descendingSlope)
            //        {
            //            // Ensure smooth transition from descending slopes
            //            collisions.descendingSlope = false;
            //            moveAmt = collisions.moveAmtOld;
            //        }

            //        float distToSlopeStart = 0;

            //        if (slopeAngle != collisions.slopeAngleOld)
            //        {
            //            // Adjust the position to start climbing the slope
            //            distToSlopeStart = hit.distance - skinWidth;
            //            moveAmt.x -= distToSlopeStart * dirX;
            //        }

            //        // Climb the slope
            //        ClimbSlope(ref moveAmt, slopeAngle, hit.normal);
            //        moveAmt.x += distToSlopeStart * dirX;
            //    }

            //    if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
            //    {
            //        moveAmt.x = (hit.distance - skinWidth) * dirX;
            //        rayLen = hit.distance;

            //        if (collisions.climbingSlope)
            //        {
            //            // Adjust vertical movement on slopes
            //            moveAmt.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmt.x);
            //        }
            //        collisions.left = dirX == -1;
            //        collisions.right = dirX == 1;
            //    }
            //}
        }
    }

    private void VerticalCollisions(ref Vector2 moveAmt)
    {
        float dirY = Mathf.Sign(moveAmt.y);
        float rayLen = Mathf.Abs(moveAmt.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (dirY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmt.x);

            // Cast a ray to check for collisions
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dirY, rayLen, collisionMask);

            // Debug visualization of raycasts
            Debug.DrawRay(rayOrigin, Vector2.up * dirY, Color.red);

            if (hit)
            {
                if (hit.collider.CompareTag("Through"))
                {
                    if (dirY == 1 || hit.distance == 0)
                    {
                        // Continue if the platform is marked as "Through" or hit point is at origin
                        continue;
                    }

                    if (collisions.fallingThroughPlatform)
                    {
                        // Continue if already falling through a platform
                        continue;
                    }

                    if (playerInput.y == -1)
                    {
                        // Start falling through the platform
                        collisions.fallingThroughPlatform = true;
                        Invoke(nameof(ResetFallingThroughPlatform), 0.5f);
                        continue;
                    }
                }

                // Adjust the vertical movement based on the collision
                moveAmt.y = (hit.distance - skinWidth) * dirY;
                rayLen = hit.distance;

                if (collisions.climbingSlope)
                {
                    // Adjust horizontal movement on slopes
                    moveAmt.x = moveAmt.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmt.x);
                }

                // Update collision flags
                collisions.below = dirY == -1;
                collisions.above = dirY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float dirX = Mathf.Sign(moveAmt.x);
            rayLen = Mathf.Abs(moveAmt.x) + skinWidth;

            Vector2 rayOrigin = ((dirX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmt.y;

            // Cast a ray to check for slope changes
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLen, collisionMask);

            if (hit)
            {
                // Calculate the slope angle of the surface
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != collisions.slopeAngle)
                {
                    // Adjust horizontal movement based on the slope
                    moveAmt.x = (hit.distance - skinWidth) * dirX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 moveAmt, float slopeAngle, Vector2 slopeNormal)
    {
        float moveDist = Mathf.Abs(moveAmt.x);
        float climbY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDist;

        if (moveAmt.y <= climbY)
        {
            // Ensure a smooth climb up the slope
            moveAmt.y = climbY;
            moveAmt.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDist * Mathf.Sign(moveAmt.x);

            // Update collision flags
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
    }

    private void DescendSlope(ref Vector2 moveAmt)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmt.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmt.y) + skinWidth, collisionMask);

        if (maxSlopeHitLeft ^ maxSlopeHitRight)
        {
            // Slide down a maximum slope
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmt);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmt);
        }

        if (!collisions.slidingDownMaxSlope)
        {
            float dirX = Mathf.Sign(moveAmt.x);
            Vector2 rayOrigin = (dirX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

            // Cast a ray to check for slope descent
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == dirX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmt.x))
                        {
                            float moveDist = Mathf.Abs(moveAmt.x);
                            float descendY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDist;

                            // Adjust movement for slope descent
                            moveAmt.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDist * Mathf.Sign(moveAmt.x);
                            moveAmt.y -= descendY;

                            // Update collision flags
                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                            collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmt)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle > maxSlopeAngle)
            {
                // Slide down the maximum slope
                moveAmt.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmt.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                // Update collision flags
                collisions.slopeAngle = slopeAngle;
                collisions.slidingDownMaxSlope = true;
                collisions.slopeNormal = hit.normal;
            }
        }
    }

    private void ResetFallingThroughPlatform()
    {
        // Reset the flag for falling through platforms
        collisions.fallingThroughPlatform = false;
    }

    // Struct to store collision information
    public struct CollisionInfo
    {
        public bool above, below, left, right; // Collision flags
        public bool climbingSlope, descendingSlope, slidingDownMaxSlope, fallingThroughPlatform; // Additional collision flags
        public float slopeAngle, slopeAngleOld; // Slope angles
        public Vector2 slopeNormal, moveAmtOld; // Slope normal and previous move amount
        public int faceDir; // Direction the chef is facing

        // Reset all collision flags
        public void Reset()
        {
            above = below = left = right = climbingSlope = descendingSlope = slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}