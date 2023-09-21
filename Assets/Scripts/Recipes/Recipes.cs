using System.Collections.Generic;

namespace Recipes
{
    public class Recipes
    {
        private static List<Ingredient> strawberryCakeEasy = new List<Ingredient>()
        {
            Ingredient.Cake,
            Ingredient.VanillaCream,
            Ingredient.Strawberries
        };
        
        private static List<Ingredient> strawberryCakeMedium = new List<Ingredient>()
        {
            Ingredient.Cake,
            Ingredient.VanillaCream,
            Ingredient.Cake,
            Ingredient.ChocolateCream,
            Ingredient.Strawberries
        };
        
        private static List<Ingredient> strawberriesAndCream = new List<Ingredient>()
        {
            Ingredient.Strawberries,
            Ingredient.VanillaCream,
            Ingredient.ChocolateCream,
            Ingredient.Strawberries
        };
        
        public static List<Recipe> recipes = new List<Recipe>()
        {
            new Recipe(
                strawberryCakeEasy,
                Difficulty.VERY_EASY,
                5
            ),
            new Recipe(
                strawberryCakeMedium,
                Difficulty.MEDIUM,
                10
            ),
            new Recipe(
                strawberriesAndCream,
                Difficulty.EASY,
                5
            )
        };
    }
}