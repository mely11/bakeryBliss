using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Vertical offset for the camera's position relative to the target.
    public float yOffset = 1.1f;

    // The distance ahead of the target that the camera should look.
    public float xLookAheadDist = 3.9f;

    // Smoothing time for the camera's horizontal movement.
    public float xLookSmoothT = 0.51f;

    // Smoothing time for the camera's vertical movement.
    public float yLookSmoothT = 0.11f;

    // The target object that the camera will follow (aka. the chef).
    public ChefController2D target;

    // The size of the focus area that the camera will use.
    public Vector2 focusSize = new(3, 5);

    private FocusArea focus;

    // Variables to handle horizontal camera movement.
    private float currentXLookAhead, targetXLookAhead, xLookAheadDir, xSmoothLookVelocity, ySmoothVelocity;
    private bool xLookAheadStopped;

    // Initialize the focus area based on the target's collider bounds and the specified size.
    private void Start() => focus = new FocusArea(target.collider.bounds, focusSize);

    private void LateUpdate()
    {
        // Update the focus area based on the target's collider bounds.
        focus.Update(target.collider.bounds);

        if (focus.velocity.x != 0)
        {
            xLookAheadDir = Mathf.Sign(focus.velocity.x);

            // Check if the player input direction matches the camera's focus area direction.
            if (xLookAheadDir == Mathf.Sign(target.playerInput.x) && target.playerInput.x != 0)
            {
                xLookAheadStopped = false;
                targetXLookAhead = xLookAheadDir * xLookAheadDist;
            }
            else
            {
                if (!xLookAheadStopped)
                {
                    xLookAheadStopped = true;
                    // Smoothly adjust the targetXLookAhead when the input direction changes.
                    targetXLookAhead = currentXLookAhead + (xLookAheadDir * xLookAheadDist - currentXLookAhead) / 4f;
                }
            }
        }

        // Smoothly adjust the camera's horizontal position.
        currentXLookAhead = Mathf.SmoothDamp(currentXLookAhead, targetXLookAhead, ref xSmoothLookVelocity, xLookSmoothT);

        // Calculate the final camera position.
        Vector2 focusPosition = focus.center + Vector2.up * yOffset;
        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref ySmoothVelocity, yLookSmoothT);
        focusPosition += Vector2.right * currentXLookAhead;

        // Apply the calculated position to the camera.
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }


    // A struct to define and update the focus area.
    private struct FocusArea
    {
        public Vector2 center, velocity;
        private float left, right, top, bottom;

        // Constructor to initialize the focus area.
        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        // Update the focus area based on the target's bounds.
        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;

            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            float shiftY = 0;

            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}