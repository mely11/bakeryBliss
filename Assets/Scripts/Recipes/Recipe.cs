using System.Collections.Generic;

namespace Recipes
{
    public struct Recipe
    {
        public List<Ingredient> recipe;
        public Difficulty difficulty;
        public int income;

        public Recipe(List<Ingredient> recipe, Difficulty difficulty, int income)
        {
            this.recipe = recipe;
            this.difficulty = difficulty;
            this.income = income;
        }

        // compare the users list of ingredients to this recipe
        public bool equalsPlayerIngredients(List<Ingredient> otherIngredients)
        {
            return false;
        }
    }
}