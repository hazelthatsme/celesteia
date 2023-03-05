using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Game;
using Celesteia.Game.Components.Items;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Collections {
    public abstract class ItemSpriteProperties {
        public const int SIZE = 16; 
    }

    public class ItemTypes {        
        private List<ItemType> Types;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading block types...");

            Types = new List<ItemType>();

            TextureAtlas atlas = TextureAtlas.Create("items",
                Content.Load<Texture2D>("sprites/items"),
                ItemSpriteProperties.SIZE,
                ItemSpriteProperties.SIZE
            );

            AddBlockItem("Stone", atlas.GetRegion(4), 1, "Tough as rock.");
            AddBlockItem("Soil", atlas.GetRegion(3), 2, "Rich in nutrients.");
            AddBlockItem("Grown Soil", atlas.GetRegion(2), 3, "Grassy.");
            AddBlockItem("Deepstone", atlas.GetRegion(5), 4, "Tougher than rock.");
            AddBlockItem("Wooden Log", atlas.GetRegion(11), 5, "Good for making planks.");
            AddBlockItem("Iron Ore", atlas.GetRegion(10), 7, "Contains nodes of iron.");
            AddBlockItem("Copper Ore", atlas.GetRegion(9), 8, "Contains nodes of copper.");
            AddIngredientItem("Coal Lump", atlas.GetRegion(14), "A primitive fuel source.");
            AddToolItem("Iron Pickaxe", atlas.GetRegion(33), 4, "Used to harvest blocks.");
            AddBlockItem("Wooden Planks", atlas.GetRegion(6), 10, "Great for building.");
            AddIngredientItem("Plank", atlas.GetRegion(15), "Good for making blocks.");
        }

        byte next = 0;
        private void AddItem(string name, string lore, TextureRegion2D sprite, ItemActions actions, bool consumeOnUse, int maxStack) {
            Types.Add(new ItemType(next, name, lore, sprite, actions, consumeOnUse, maxStack));
            next++;
        }

        private void AddBlockItem(string name, TextureRegion2D sprite, byte blockID, string lore) {
            AddItem(name, lore, sprite, new BlockItemActions(blockID), true, 99);
        }

        private void AddToolItem(string name, TextureRegion2D sprite, int power, string lore) {
            AddItem(name, lore, sprite, new PickaxeItemActions(power), false, 1);
        }

        private void AddIngredientItem(string name, TextureRegion2D sprite, string lore) {
            AddItem(name, lore, sprite, null, true, 99);
        }

        public ItemType GetItem(byte id) {
            return Types.Find(x => x.ItemID == id);
        }

        public ItemType GetItem(string name) {
            return Types.Find(x => x.Name == name);
        }
    }

    public class ItemType {
        public readonly byte ItemID;
        public readonly string Name;
        public readonly string Lore;
        public readonly TextureRegion2D Sprite;
        public readonly int MaxStackSize;
        public ItemActions Actions;
        public readonly bool ConsumeOnUse;

        public ItemType(byte id, string name, string lore, TextureRegion2D sprite, ItemActions actions, bool consumeOnUse, int maxStack) {
            Debug.WriteLine($"  Loading item '{name}' ({id})...");

            ItemID = id;
            Name = name;
            Lore = lore;
            Sprite = sprite;
            Actions = actions;
            MaxStackSize = maxStack;
            ConsumeOnUse = consumeOnUse;
        }

        public ItemStack GetStack(int amount) {
            return new ItemStack(ItemID, amount);
        }
    }
}