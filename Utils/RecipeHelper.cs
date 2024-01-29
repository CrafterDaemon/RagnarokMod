using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Utils
{
    internal class GetRecipe
    {

        internal Recipe[] Recipes;

        internal int Result;

        internal int Stack;
        public GetRecipe()
        {
            //gets all the recipes
            Recipes = Main.recipe;
        }

        public void LookFor(int result, int stack)
        {
            //sets target recipe(s)
            Result = result;
            Stack = stack;
        }

        public List<Recipe> Search()
        {
            //locate the recipe
            List<Recipe> recipelist = new List<Recipe>();

            foreach (Recipe recipe in Recipes)
            {
                if (recipe.createItem.type == Result)
                {
                    recipe.ReplaceResult(recipe.createItem.type, Stack);
                    recipelist.Add(recipe);
                }
            }
            return recipelist;
        }
    }

    internal class RecipeHelper
    {
        internal Recipe Result;

        public RecipeHelper(Recipe recipe)
        {
            Result = recipe;
        }

        public void Remove(int IngredientType)
        {
            Result.RemoveIngredient(IngredientType);
        }

        public void Add(int IngredientType, int amount)
        {
            Result.AddIngredient(IngredientType, amount);
        }
        public void Disable()
        {
            Result.DisableRecipe();
        }
    }

    internal class SimpleRecipeHandler
    {
        //for easier recipes that i am not writing by hand
        public void SimpleRecipe(int result, int ing1, int stack1, int ing2, int stack2, int station)
        {
            Recipe.Create(result)
                  .AddIngredient(ing1, stack1)
                  .AddIngredient(ing2, stack2)
                  .AddTile(station)
                  .Register();
        }
        //same as above, but now 20% simpler!
        public void SimpleRecipe(int result, int ing1, int stack1, int station)
        {
            Recipe.Create(result)
                  .AddIngredient(ing1, stack1)
                  .AddTile(station)
                  .Register();
        }
    }
}
