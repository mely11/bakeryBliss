using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    public int maxPatience = 100;   // Maximum patience value
    public int currentPatience;     // Current patience value
    public float duration = 30f;    // Duration in seconds for the patience to reach zero
    public Slider slider;           // Reference to the UI Slider
    public Gradient gradient;       // Gradient for the patience bar color
    public Image fill;              // Reference to the UI Image for patience bar fill

    private float initialPatience;  // Initial patience value
    private float startTime;        // Time when the countdown starts
    private float patienceRate;     // Rate at which patience decreases per second

    void Start()
    {
        // Initialize current patience to the maximum value
        currentPatience = maxPatience;

        // Set the maximum patience value for the UI slider
        SetMaxPatience(maxPatience);

        // Record the start time when the countdown begins
        startTime = Time.time;

        // Calculate the initial patience based on the duration
        initialPatience = maxPatience;

        // Calculate the rate at which patience decreases per second
        patienceRate = maxPatience / duration;
    }

    void Update()
    {
        // Calculate the elapsed time since the countdown started
        float elapsedTime = Time.time - startTime;

        // Calculate the current patience based on the initial patience and elapsed time
        float current = Mathf.Max(initialPatience - (elapsedTime * patienceRate), 0);

        // Update the patience value and UI
        SetPatience(Mathf.RoundToInt(current));

        if (current <= 0)
        {
            // When the countdown ends, call the OnEnd function
            OnEnd();
        }
    }

    void OnEnd()
    {
        // This function is called when the countdown timer ends
        // TODO: You can perform any desired actions here
        Debug.Log("The customer's patience just ran out!");
    }

    public void SetMaxPatience(int patience)
    {
        // Set the maximum value for the UI slider
        slider.maxValue = patience;

        // Initialize the slider value to the maximum patience value
        slider.value = patience;

        // Set the initial color for the patience bar fill using the gradient
        fill.color = gradient.Evaluate(1f);
    }

    public void SetPatience(int patience)
    {
        // Update the UI slider to reflect the current patience value
        slider.value = patience;

        // Update the color of the patience bar fill based on the normalized patience value and gradient
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}