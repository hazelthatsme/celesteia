using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
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

            Types.Add(new ItemType(0, "Stone", atlas.GetRegion(4)));
            Types.Add(new ItemType(1, "Soil", atlas.GetRegion(3)));
            Types.Add(new ItemType(2, "Grown Soil", atlas.GetRegion(2)));
            Types.Add(new ItemType(3, "Deepstone", atlas.GetRegion(5)));
            Types.Add(new ItemType(4, "Wooden Log", atlas.GetRegion(11)));
            Types.Add(new ItemType(6, "Iron Ore", atlas.GetRegion(10)));
            Types.Add(new ItemType(7, "Copper Ore", atlas.GetRegion(9)));
            Types.Add(new ItemType(8, "Coal Lump", atlas.GetRegion(14)));
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

        public ItemType(byte id, string name, TextureRegion2D sprite, int maxStack = 99) {
            Debug.WriteLine($"  Loading item '{name}'...");

            ItemID = id;
            Name = name;
            Sprite = sprite;
            MaxStackSize = maxStack;
        }
    }
}