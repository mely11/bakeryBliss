using Recipes;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    // Define the customer's attributes
    public GameObject customer;
    public float patienceDuration = 30.0f; // Maximum patience duration for this customer
    public int paymentAmount = 10; // Dollar amount for successfully completing the order

    private float patienceTimer; // Timer for tracking patience
    private bool hasOrderBeenFulfilled;

    private ScoreManager _scoreManager;
    private ChefCollisions _playerCollisions;
    private SpriteController spriteController;
    private PatienceBar patienceBar; 
    private Recipe order; // List of ingredients in the customer's order
    
    // Initialize the customer's attributes
    void Start()
    {
        // Get references to controllers we need to do operations
        spriteController = GameObject.Find("GameInfoCanvas").GetComponent<SpriteController>();
        InitializeAndDisplayCustomer();
        InitializePatienceBar();
        order = customer.GetComponent<RecipeController>().customerOrder;
        GameObject player = GameObject.Find("Player");
        Debug.Log(player);
        _playerCollisions = GameObject.Find("Player").GetComponent<ChefCollisions>();
        _scoreManager = GameObject.Find("Player").GetComponent<ScoreManager>();
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
                //OnPatienceRunOut();
            }
            if (_playerCollisions.collectedIngredients.Count == order.recipe.Count)
            {
                if (_playerCollisions.compare(order))
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
        //_scoreManager.AddToScore(paymentAmount);

        hasOrderBeenFulfilled = true;
        // Destroy the entire CustomerParent GameObject, including both customer and patience bar
        //Destroy(transform.parent.gameObject);
    }

    
    // Called when patience runs out
    private void OnPatienceRunOut()
    {
        // Deduct some payment for running out of patience (optional)
        _scoreManager.DeductFromScore(paymentAmount / 2);

        // Destroy the entire CustomerParent GameObject, including both customer and patience bar
        Destroy(transform.parent.gameObject);
    }

    private void InitializePatienceBar()
    {
        patienceBar = customer.GetComponent<PatienceBar>();
        patienceTimer = patienceDuration;
        patienceBar.Initialize(patienceDuration);
    }

    private void InitializeAndDisplayCustomer()
    {
        customer = new GameObject("Customer");
        customer.transform.SetParent(gameObject.transform);
        customer.AddComponent<RectTransform>();
        RectTransform rectTransform = customer.GetComponent<RectTransform>();
        rectTransform.SetParent(this.gameObject.GetComponent<RectTransform>().transform); // child of the CustomerPanel created in CustomerQueueController
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 66, 64);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 128);
        //customer.AddComponent<CanvasRenderer>();
        customer.AddComponent<Image>();
        customer.AddComponent<PatienceBar>();
        customer.AddComponent<RecipeController>();
        Image customerImage = customer.GetComponent<Image>();
        customerImage.sprite = spriteController.happyCustomer;
    }

    // Update the patience bar's visual representation
    private void UpdatePatienceBar()
    {
        // Update the UI to reflect the remaining patience time
        //patienceBar.UpdatePatience(patienceTimer);
    }
}
