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
    private SpriteController _spriteController;

    [SerializeField] private GameObject customerParent;

    void Start()
    {
        _spriteController = GameObject.Find("GameInfoCanvas").GetComponent<SpriteController>();
        CreatePatienceBar();
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
        // Deactivate the customer parent GameObject
        customerParent.SetActive(false);
    }
    public void Initialize(float patienceDuration)
    {
        // Set the duration for the patience bar
        duration = patienceDuration;

        // Initialize other properties or UI elements as needed
        SetMaxPatience(maxPatience);
    }

    public void UpdatePatience(float currentPatience)
    {
        // Update the UI to reflect the current patience value
        SetPatience(Mathf.RoundToInt(currentPatience));
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

    private void CreatePatienceBar()
    {
        GameObject patienceBar = new GameObject("PatienceBar");
        patienceBar.transform.SetParent(gameObject.transform);
        
        // set position within the parent object
        patienceBar.AddComponent<RectTransform>();
        RectTransform rectTransform = patienceBar.GetComponent<RectTransform>();
        rectTransform.SetParent(this.gameObject.GetComponent<RectTransform>().transform);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -68, 64);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 10);
        
        // assign sprite image
        patienceBar.AddComponent<CanvasRenderer>();
        patienceBar.AddComponent<Image>();
        Image image = patienceBar.GetComponent<Image>();
        image.sprite = _spriteController.patienceBar;
    }
}


// * -------------------------------------- Don't Delete -------------------------------------- *
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//public class PatienceBar : MonoBehaviour
//{
//    // Public variables that can be set in the Unity Inspector
//    public int maxPatience = 100;  // Maximum patience value
//    public int currentPatience;             // Current patience value
//    public float duration = 30f;          // Duration in seconds for the patience to reach zero
//    public Slider slider;                 // Reference to the UI Slider
//    public Gradient gradient;             // Gradient for the patience bar color
//    public Image fill;                    // Reference to the UI Image for patience bar fill

//    // Private variables for internal calculations only
//    private float damagePerFrame;         // Damage to apply per frame
//    private float startTime;              // Time when the patience bar countdown starts

//    void Start()
//    {
//        // Initialize current patience to the maximum value
//        currentPatience = maxPatience;

//        // Set the maximum patience value for the UI slider
//        SetMaxPatience(maxPatience);

//        // Record the start time when the countdown begins
//        startTime = Time.time;

//        // Start the countdown timer using a coroutine
//        StartCoroutine(UpdateTimer());
//    }

//    //private IEnumerator UpdateTimer()
//    //{
//    //    // Calculate the fixed damage per frame
//    //    float frameRate = 1f / Time.deltaTime;
//    //    float fixedDamagePerFrame = maxPatience / (duration * frameRate);

//    //    while (currentPatience > 0)
//    //    {
//    //        // Apply the fixed damage per frame
//    //        TakeDamage(Mathf.RoundToInt(fixedDamagePerFrame));

//    //        // Yield to the next frame
//    //        yield return null;
//    //    }

//    //    // When the countdown ends, call the OnEnd function
//    //    OnEnd();
//    //}

//    //private IEnumerator UpdateTimer()
//    //{
//    //    float remainingTime = duration;

//    //    while (remainingTime > 0)
//    //    {
//    //        // Calculate the damage per frame based on the remaining time
//    //        float damagePerFrame = maxPatience / duration;

//    //        // Calculate the damage to apply during this frame
//    //        float damageThisFrame = damagePerFrame * Time.deltaTime;

//    //        // Ensure the damage doesn't exceed the remaining patience
//    //        damageThisFrame = Mathf.Min(damageThisFrame, currentPatience);

//    //        // Apply the damage and update the patience
//    //        TakeDamage(Mathf.RoundToInt(damageThisFrame));

//    //        // Decrease the remaining time
//    //        remainingTime -= Time.deltaTime;

//    //        // Yield to the next frame
//    //        yield return null;
//    //    }

//    //    // When the countdown ends, call the OnEnd function
//    //    OnEnd();
//    //}

//    private IEnumerator UpdateTimer()
//    {
//        while (currentPatience > 0)
//        {
//            // Calculate the elapsed time since the countdown started
//            float elapsedTime = Time.time - startTime;

//            // Calculate the current damage per frame based on the actual frame rate
//            float frameRate = 1f / Time.deltaTime;
//            damagePerFrame = maxPatience / (duration * frameRate);

//            // Calculate the damage to apply during this frame
//            float damageThisFrame = damagePerFrame * elapsedTime;

//            // Ensure the damage doesn't exceed the remaining patience
//            damageThisFrame = Mathf.Min(damageThisFrame, currentPatience);

//            // Apply the damage and update the patience
//            TakeDamage(Mathf.RoundToInt(damageThisFrame));

//            // Yield to the next frame
//            yield return null;
//        }

//        // When the countdown ends, call the OnEnd function
//        OnEnd();
//    }

//    void TakeDamage(int damage)
//    {
//        // Decrease the current patience by the specified damage amount
//        currentPatience -= damage;

//        // Update the UI to reflect the new patience value
//        SetPatience(currentPatience);
//    }

//    private void OnEnd()
//    {
//        // This function is called when the countdown timer ends
//        // TODO: You can perform any desired actions here
//        print("The customer's patience just ran out!");
//    }

//    public void SetMaxPatience(int patience)
//    {
//        // Set the maximum value for the UI slider
//        slider.maxValue = patience;

//        // Initialize the slider value to the maximum patience value
//        slider.value = patience;

//        // Set the initial color for the patience bar fill using the gradient
//        fill.color = gradient.Evaluate(1f);
//    }

//    public void SetPatience(int patience)
//    {
//        // Update the UI slider to reflect the current patience value
//        slider.value = patience;

//        // Update the color of the patience bar fill based on the normalized patience value and gradient
//        fill.color = gradient.Evaluate(slider.normalizedValue);
//    }
//}