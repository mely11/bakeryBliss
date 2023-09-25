using Recipes;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RecipeController : MonoBehaviour
{
    public Recipe customerOrder;
    private SpriteController spriteController;

    // Start is called before the first frame update
    void Start()
    {
        spriteController = GameObject.Find("GameInfoCanvas").GetComponent<SpriteController>();
        customerOrder = getCustomerOrder();
        createIngredientGameObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Recipe getCustomerOrder()
    {
        Random rnd = new Random();
        int num = rnd.Next(Recipes.Recipes.recipes.Count);

        return Recipes.Recipes.recipes[num];
    }

    void createIngredientGameObjects()
    {
        for (int i = 0; i < customerOrder.recipe.Count; i++)
        {
            Ingredient ingredient = customerOrder.recipe[i];
            GameObject currentIngredientGameObject = new GameObject("ingredient" + i);
            currentIngredientGameObject.transform.SetParent(this.gameObject.transform);
            currentIngredientGameObject.AddComponent<RectTransform>();
            RectTransform rectTransform = currentIngredientGameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(this.gameObject.GetComponent<RectTransform>().transform); // child of the CustomerPanel created in CustomerQueueController
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 90, 100);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,  100 - (i * 30), 30);
            currentIngredientGameObject.AddComponent<CanvasRenderer>();
            currentIngredientGameObject.AddComponent<Image>();
            Image image = currentIngredientGameObject.GetComponent<Image>();
            switch (ingredient)
            {
                case Ingredient.Cake:
                    image.sprite = spriteController.cake;
                    break;
                case Ingredient.Strawberries:
                    image.sprite = spriteController.strawberry;
                    break;
                case Ingredient.VanillaCream:
                    image.sprite = spriteController.vanillaCream;
                    break;
                case Ingredient.ChocolateCream:
                    image.sprite = spriteController.chocolateCream;
                    break;
            }
        }
    }
}