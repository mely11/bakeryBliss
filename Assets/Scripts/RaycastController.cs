// https://gamedevbeginner.com/raycasts-in-unity-made-easy/#:~:text=Raycast%20in%20Unity%20is%20a,Hit%20variable%20for%20further%20use
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;

    public const float skinWidth = .015f; // The amount by which the collider is expanded for raycasting.
    private const float raySpacing = .25f; // Distance between rays

    [HideInInspector]
    public int horizontalRayCount, verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing, verticalRaySpacing;

    [HideInInspector]
    public new BoxCollider2D collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Awake() => collider = GetComponent<BoxCollider2D>();

    public virtual void Start() => CalculateRaySpacing();

    // Update the origins of the raycasting rays based on the collider bounds.
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2); // Shrink the bounds by the skinWidth.

        // Define the corners of the collider
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    // Calculate the spacing and count of raycasting rays.
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2); // Shrink the bounds by the skinWidth.

        // Calculate the number of rays and their spacing based on the collider size.
        horizontalRayCount = Mathf.RoundToInt(bounds.size.y / raySpacing);
        verticalRayCount = Mathf.RoundToInt(bounds.size.x / raySpacing);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    // Struct to store the origins of raycasting rays.
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }
}