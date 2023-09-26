using Recipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    // Define the customer's attributes
    public GameObject customer;
    public int paymentAmount = 10; // Dollar amount for successfully completing the order

    private bool hasOrderBeenFulfilled;

    private ScoreManager _scoreManager;
    private ChefCollisions _playerCollisions;
    private SpriteController spriteController;
    private PatienceBar patienceBar; 
    private Recipe order; // List of ingredients in the customer's order
    private GameObject patienceBarInstance;
    
    // Initialize the customer's attributes
    void Start()
    {
        // Get references to controllers we need to do operations
        spriteController = GameObject.Find("GameInfoCanvas").GetComponent<SpriteController>();
        InitializeAndDisplayCustomer();
        
        _playerCollisions = GameObject.FindWithTag("Player").GetComponent<ChefCollisions>();
        _scoreManager = GameObject.FindWithTag("Player").GetComponent<ScoreManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Decrement patience over time
        if (!hasOrderBeenFulfilled)
        {
            // Check if patience has run out
            if (patienceBar.currentPatience <= 0.0f)
            {
                OnPatienceRunOut();
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
        
        // set order
        customer.AddComponent<RecipeController>();
        customer.GetComponent<RecipeController>().SetCustomerOrder();
        order = customer.GetComponent<RecipeController>().customerOrder;

        // Set customer image
        customer.AddComponent<Image>();
        Image customerImage = customer.GetComponent<Image>();
        customerImage.sprite = spriteController.happyCustomer;

        GameObject patienceBarPrefab = spriteController.patienceBarPrefab;
        patienceBarInstance = Instantiate(patienceBarPrefab, new Vector3(), Quaternion.identity);
        patienceBarInstance.transform.SetParent(gameObject.transform);
        patienceBar = patienceBarInstance.GetComponent<PatienceBar>();
        RectTransform rectTransform2 = patienceBarInstance.GetComponent<RectTransform>();
        rectTransform2.SetParent(gameObject.transform.GetComponent<RectTransform>().transform); // child of the CustomerPanel created in CustomerQueueController
        rectTransform2.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10, 80);
        rectTransform2.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 20, 20);
    }
    
}
