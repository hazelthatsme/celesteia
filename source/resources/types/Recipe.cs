using System.Collections.Generic;
using Celesteia.Game.Components.Items;

namespace Celesteia.Resources.Types {
    public class Recipe : IResourceType {
        private byte id;
        public readonly string Name;
        public byte GetID() => id;
        public void SetID(byte value) => id = value;

        public Part Result;
        public List<Part> Ingredients;

        public Recipe(Part result, params Part[] ingredients) {
            Result = result;
            Ingredients = new List<Part>(ingredients);
        }

        public bool Craftable(Inventory inventory) {
            foreach (Part ingredient in Ingredients)
                if (!inventory.ContainsAmount(ingredient.Key, ingredient.Amount)) return false;

            return true;
        }

        public void Craft(Inventory inventory) {
            if (!Craftable(inventory)) return;

            foreach (Part ingredient in Ingredients)
                inventory.RemoveAmount(ingredient.Key, ingredient.Amount);

            inventory.AddItem(Result.Stack);
        }
    }

    public struct Part {
        public NamespacedKey Key;
        public int Amount;

        public Part(NamespacedKey key, int amount) {
            Key = key;
            Amount = amount;
        }

        public ItemStack Stack => new ItemStack(Key, Amount);
        public ItemType GetItemType() => ResourceManager.Items.GetResource(Key) as ItemType;
    }
}