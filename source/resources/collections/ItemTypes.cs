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

            AddBlockItem("Stone", atlas.GetRegion(4), 1);
            AddBlockItem("Soil", atlas.GetRegion(3), 2);
            AddBlockItem("Grown Soil", atlas.GetRegion(2), 3);
            AddBlockItem("Deepstone", atlas.GetRegion(5), 4);
            AddBlockItem("Wooden Log", atlas.GetRegion(11), 5);
            AddBlockItem("Iron Ore", atlas.GetRegion(10), 7);
            AddBlockItem("Copper Ore", atlas.GetRegion(9), 8);
            AddIngredientItem("Coal Lump", atlas.GetRegion(14));
            AddToolItem("Iron Pickaxe", atlas.GetRegion(33), 4);
            AddBlockItem("Wooden Planks", atlas.GetRegion(6), 10);
            AddIngredientItem("Plank", atlas.GetRegion(15));
        }

        byte next = 0;
        private void AddItem(string name, TextureRegion2D sprite, ItemActions actions, bool consumeOnUse, int maxStack) {
            Types.Add(new ItemType(next, name, sprite, actions, consumeOnUse, maxStack));
            next++;
        }

        private void AddBlockItem(string name, TextureRegion2D sprite, byte blockID) {
            AddItem(name, sprite, new BlockItemActions(blockID), true, 99);
        }

        private void AddToolItem(string name, TextureRegion2D sprite, int power) {
            AddItem(name, sprite, new PickaxeItemActions(power), false, 1);
        }

        private void AddIngredientItem(string name, TextureRegion2D sprite) {
            AddItem(name, sprite, null, true, 99);
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
        public readonly TextureRegion2D Sprite;
        public readonly int MaxStackSize;
        public ItemActions Actions;
        public readonly bool ConsumeOnUse;

        public ItemType(byte id, string name, TextureRegion2D sprite, ItemActions actions, bool consumeOnUse, int maxStack) {
            Debug.WriteLine($"  Loading item '{name}' ({id})...");

            ItemID = id;
            Name = name;
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