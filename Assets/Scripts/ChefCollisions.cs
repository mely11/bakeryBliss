using System.Collections.Generic;
using Recipes;
using UnityEngine;

public class ChefCollisions : MonoBehaviour
{
    public List<Ingredient> collectedIngredients;
    
    // Start is called before the first frame update
    void Start()
    {
        collectedIngredients = new List<Ingredient>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.name)
        {
            case "Cake":
                collectedIngredients.Add(Ingredient.Cake);
                Destroy(other.gameObject);
                break;
            case "ChocolateCreamIngredient":
                collectedIngredients.Add(Ingredient.ChocolateCream);
                Destroy(other.gameObject);
                break;
            case "VanillaCreamIngredient":
                collectedIngredients.Add(Ingredient.VanillaCream);
                Destroy(other.gameObject);
                break;
            case "StrawberryIngredient":
                collectedIngredients.Add(Ingredient.Strawberries);
                Destroy(other.gameObject);
                break;
        }
    }

    public bool compare(Recipe order)
    {
        return order.equalsPlayerIngredients(collectedIngredients);
    }

    public void clearList()
    {
        collectedIngredients.Clear();
    }
}
