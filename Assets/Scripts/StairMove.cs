using UnityEngine;

public class StairMove : MonoBehaviour
{
    public float basePositionChange = 2.0f; // Base speed of movement per second
    public float speedVariation = 1.0f; // Maximum speed variation
    private float positionChange; // Randomly determined speed
    private bool ascending = true; // Indicates if the obstacle is currently moving up or down
    private Vector3 initialPosition; // The initial position of the obstacle

    private void Start()
    {
        initialPosition = transform.position; // Store the initial position

        // Calculate a random speed with minimal variation
        positionChange = basePositionChange + Random.Range(-speedVariation, speedVariation);
    }

    void Update()
    {
        float upperLimit = initialPosition.y + 3f; // Upper limit for obstacle's movement
        float lowerLimit = initialPosition.y - 2f; // Lower limit for obstacle's movement

        if (ascending && transform.position.y < upperLimit)
        {
            // Move the obstacle upward if it hasn't reached the upper limit
            transform.position += positionChange * Time.deltaTime * Vector3.up;
        }
        else if (!ascending && transform.position.y > lowerLimit)
        {
            // Move the obstacle downward if it hasn't reached the lower limit
            transform.position -= positionChange * Time.deltaTime * Vector3.up;
        }
        else
        {
            // Change direction if the obstacle has reached either limit
            ascending = !ascending;
        }
    }
}