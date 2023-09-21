using System;
using Recipes;
using UnityEngine;
using Random = System.Random;

public class RecipeController : MonoBehaviour
{
    private Recipe customerOrder;
    public GameObject prefabCake;
    public GameObject prefabVanillaCream;
    public GameObject prefabChocolateCream;
    public GameObject prefabStrawberries;

    private bool moving = true;
    
    // Start is called before the first frame update
    void Start()
    {
        customerOrder = getRecipe();
        createIngredientGameObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 0 && moving)
        {
            transform.Translate(Vector3.right * Time.deltaTime * 2.0f);
        }
    }

    Recipe getRecipe()
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
            switch (ingredient)
            {
                case Ingredient.Cake:
                    Instantiate(
                        prefabCake,
                        new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, 0.0f),
                        Quaternion.identity
                    ).transform.SetParent(this.transform);
                    break;
                case Ingredient.Strawberries:
                    Instantiate(
                        prefabStrawberries,
                        new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, 0.0f),
                        Quaternion.identity
                    ).transform.SetParent(this.transform);
                    break;
                case Ingredient.VanillaCream:
                    Instantiate(
                        prefabVanillaCream,
                        new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, 0.0f),
                        Quaternion.identity
                    ).transform.SetParent(this.transform);
                    break;
                case Ingredient.ChocolateCream:
                    Instantiate(
                        prefabChocolateCream,
                        new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, 0.0f),
                        Quaternion.identity
                    ).transform.SetParent(this.transform);
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Plate"))
        {
            moving = false;
        }
    }
}
