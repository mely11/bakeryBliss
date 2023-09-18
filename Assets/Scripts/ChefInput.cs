using UnityEngine;

[RequireComponent(typeof(Chef))]
public class ChefInput : MonoBehaviour
{
    private Chef chef;

    private void Start() => chef = GetComponent<Chef>();

    private void Update()
    {
        // Read directional input from the player.
        Vector2 inputDir = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        chef.SetDirectionalInput(inputDir);

        // Handle jump input.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            chef.OnJumpDown();
        }

        // Handle releasing the jump button.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            chef.OnJumpUp();
        }
    }
}