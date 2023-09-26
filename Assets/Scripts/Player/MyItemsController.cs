using System;
using Recipes;
using UnityEngine;
using UnityEngine.UI;

public class MyItemsController : MonoBehaviour
{
    private GameObject myItemsObject;
    private ChefCollisions _playerCollisions;
    private SpriteController _spriteController;
    private int nextItem;
    // Start is called before the first frame update
    void Start()
    {
        CreateEmptyMyItemsDisplayObject();
        _spriteController = GameObject.Find("GameInfoCanvas").GetComponent<SpriteController>();
        _playerCollisions = GameObject.FindWithTag("Player").GetComponent<ChefCollisions>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (_playerCollisions.collectedIngredients[nextItem] != null)
            {
                // we have a new item that the player collected in the collected ingredients list, draw it
                DrawItem(nextItem);
                nextItem++;
            }
        }
        catch (Exception e) { }
    }

    private void DrawItem(int nextItemId)
    {
        Ingredient ingredient = _playerCollisions.collectedIngredients[nextItemId];
        GameObject currentIngredientGameObject = new GameObject("ingredient" + nextItemId);
        currentIngredientGameObject.transform.SetParent(myItemsObject.transform);
        currentIngredientGameObject.AddComponent<RectTransform>();
        RectTransform rectTransform = currentIngredientGameObject.GetComponent<RectTransform>();
        rectTransform.SetParent(myItemsObject.GetComponent<RectTransform>().transform); 
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 100);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,  100 - (nextItemId * 30), 30);
        currentIngredientGameObject.AddComponent<CanvasRenderer>();
        currentIngredientGameObject.AddComponent<Image>();
        Image image = currentIngredientGameObject.GetComponent<Image>();
        switch (ingredient)
        {
            case Ingredient.Cake:
                image.sprite = _spriteController.cake;
                break;
            case Ingredient.Strawberries:
                image.sprite = _spriteController.strawberry;
                break;
            case Ingredient.VanillaCream:
                image.sprite = _spriteController.vanillaCream;
                break;
            case Ingredient.ChocolateCream:
                image.sprite = _spriteController.chocolateCream;
                break;
        }
    }

    public void DestroyMyItemsUI()
    {
        Destroy(myItemsObject);
        CreateEmptyMyItemsDisplayObject();
    }

    private void CreateEmptyMyItemsDisplayObject()
    {
        myItemsObject = new GameObject("MyItemsDisplay");
        myItemsObject.AddComponent<CanvasRenderer>();
        myItemsObject.AddComponent<RectTransform>();
        RectTransform rectTransform = myItemsObject.GetComponent<RectTransform>();
        rectTransform.SetParent(this.gameObject.GetComponent<RectTransform>().transform);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 160, 128);
        rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 20, 128);
    }
}
