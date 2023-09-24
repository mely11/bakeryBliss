using System.Collections.Generic;
using Recipes;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    // Define the customer's attributes
    public Recipe order; // List of ingredients in the customer's order
    public float patienceDuration = 30.0f; // Maximum patience duration for this customer
    public int paymentAmount = 10; // Dollar amount for successfully completing the order
    public PatienceBar patienceBar; // Reference to the patience bar UI

    private float patienceTimer; // Timer for tracking patience
    private bool hasOrderBeenFulfilled = false;
    //customer queue controller

    private ChefCollisions playerCollision;
    private RecipeController recipe;
    
    // Initialize the customer's attributes
    void Start()
    {
        patienceTimer = patienceDuration;
        DisplayPatienceBar();
        // initialize the order variable here and assign recipe
        
    }
    
    // Update is called once per frame
    void Update()
    {
        // Decrement patience over time
        if (!hasOrderBeenFulfilled)
        {
            patienceTimer -= Time.deltaTime;
            UpdatePatienceBar();

            // Check if patience has run out
            if (patienceTimer <= 0.0f)
            {
                OnPatienceRunOut();
            }
            if (playerCollision.collectedIngredients.Count == order.recipe.Count)
            {
                if (playerCollision.compare(order))
                {
                    FulfillOrder();
                }
                else
                {
                    //did not fulfill the order

                }
            }
          
       
        }
    }


    // Called when the customer's order is fulfilled
    private void FulfillOrder()
    {
       
        // Award the payment amount to the chef
        ScoreManager.Instance.AddToScore(paymentAmount);

        hasOrderBeenFulfilled = true;
        // Destroy the entire CustomerParent GameObject, including both customer and patience bar
        //Destroy(transform.parent.gameObject);
    }



    // Called when patience runs out
    private void OnPatienceRunOut()
    {
        // Deduct some payment for running out of patience (optional)
        ScoreManager.Instance.DeductFromScore(paymentAmount / 2);

        // Destroy the entire CustomerParent GameObject, including both customer and patience bar
        Destroy(transform.parent.gameObject);
    }
    // Display the patience bar for this customer
    private void DisplayPatienceBar()
    {
        // Create and initialize the patience bar UI
        patienceBar.Initialize(patienceDuration);
    }

    // Update the patience bar's visual representation
    private void UpdatePatienceBar()
    {
        // Update the UI to reflect the remaining patience time
        patienceBar.UpdatePatience(patienceTimer);
    }


}
