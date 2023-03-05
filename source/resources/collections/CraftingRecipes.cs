using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Game.Components.Items;
using Microsoft.Xna.Framework.Content;

namespace Celesteia.Resources.Collections {
    public class CraftingRecipes {        
        private List<CraftingRecipe> Recipes;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading block types...");

            Recipes = new List<CraftingRecipe>();

            AddRecipe(ResourceManager.Items.GetItem("Plank").GetStack(4), ResourceManager.Items.GetItem("Wooden Log").GetStack(1));
            AddRecipe(ResourceManager.Items.GetItem("Wooden Planks").GetStack(1), ResourceManager.Items.GetItem("Plank").GetStack(2));
        }

        byte next = 0;
        private void AddRecipe(ItemStack result, params ItemStack[] ingredients) {
            Recipes.Add(new CraftingRecipe(next, result, ingredients));
            next++;
        }

        public CraftingRecipe GetRecipe(byte id) {
            return Recipes.Find(x => x.RecipeID == id);
        }

        public List<CraftingRecipe> GetRecipes() {
            return Recipes;
        }
    }

    public class CraftingRecipe {
        public readonly byte RecipeID;
        public readonly List<ItemStack> Ingredients;
        public readonly ItemStack Result;

        public CraftingRecipe(byte id, ItemStack result, params ItemStack[] ingredients) {
            Debug.WriteLine($"  Loading recipe for '{result.Type.Name}' ({id})...");

            RecipeID = id;
            Result = result;
            Ingredients = new List<ItemStack>(ingredients);
        }

        public void TryCraft(Inventory inventory) {
            bool canCraft = true;

            foreach (ItemStack ingredient in Ingredients)
                if (!inventory.ContainsStack(ingredient)) canCraft = false;

            if (!canCraft) return;

            foreach (ItemStack ingredient in Ingredients)
                inventory.RemoveStack(ingredient);

            inventory.AddItem(Result.Clone());
        }
    }
}