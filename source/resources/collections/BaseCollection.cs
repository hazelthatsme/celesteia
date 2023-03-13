using System.Collections.Generic;
using Celesteia.Resources.Types;
using Celesteia.Resources.Types.Builders;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

/*
    A collection of resources for the base game.
*/
namespace Celesteia.Resources.Collections {
    public class BaseCollection : IResourceCollection
    {
        public Dictionary<NamespacedKey, BlockType> GetBlocks() => blocks;
        public Dictionary<NamespacedKey, ItemType> GetItems() => items;

        public Dictionary<NamespacedKey, BlockType> blocks;
        public Dictionary<NamespacedKey, ItemType> items;
        public void LoadContent(ContentManager Content) {
            LoadBlocks(Content);
        }

        private void LoadBlocks(ContentManager Content, int pixelSize = 8) {
            TextureAtlas _atlas = TextureAtlas.Create("blocks", Content.Load<Texture2D>("sprites/blocks"), pixelSize, pixelSize);
            BlockTypeBuilder builder = new BlockTypeBuilder(_atlas);
            
            blocks = new Dictionary<NamespacedKey, BlockType>();
            blocks.Add(NamespacedKey.Base("air"), new BlockType("Air") { BoundingBox = null, Strength = 0, Light = null, Translucent = true });
            blocks.Add(NamespacedKey.Base("stone"), new BlockType("Stone") { DropKey = NamespacedKey.Base("stone"), Strength = 5 });
            blocks.Add(NamespacedKey.Base("soil"), new BlockType("Soil") { DropKey = NamespacedKey.Base("soil"), Strength = 3 });
            blocks.Add(NamespacedKey.Base("grown_soil"), new BlockType("Grown Soil") { DropKey = NamespacedKey.Base("grown_soil"), Strength = 3 });
            blocks.Add(NamespacedKey.Base("deepstone"), new BlockType("Deepstone") { DropKey = NamespacedKey.Base("deepstone"), Strength = -1 });
            blocks.Add(NamespacedKey.Base("log"), new BlockType("Wooden Log") { DropKey = NamespacedKey.Base("log"), Strength = 2 });
            blocks.Add(NamespacedKey.Base("leaves"), new BlockType("Leaves") { DropKey = NamespacedKey.Base("log"), Strength = 2 });

            
            /*
                AddStandardBlock("stone", "Stone", 2, 1, NamespacedKey.Base("stone"), 5);
                AddStandardBlock("soil", "Soil", 1, 1, NamespacedKey.Base("soil"), 5);
                AddStandardBlock("grown_soil", "Grown Soil", 0, 1, NamespacedKey.Base("soil"), 5);
                AddStandardBlock("deepstone", "Deepstone", 3, 1, NamespacedKey.Base("deepstone"), -1);
                AddStandardBlock("log", "Wooden Log", 10, 1, NamespacedKey.Base("log"), 3);
                AddWalkthroughBlock("leaves", "Leaves", 11, 1, null, 1, new BlockLightProperties(LightColor.black, 0, false));
                AddStandardBlock("iron_ore", "Iron Ore", 8, 1, NamespacedKey.Base("iron_ore"), 10, new BlockLightProperties(new LightColor(63f, 63f, 63f), 4));
                AddStandardBlock("copper_ore", "Copper Ore", 7, 1, NamespacedKey.Base("copper_ore"), 10, new BlockLightProperties(new LightColor(112f, 63f, 46f), 4));
                AddStandardBlock("coal_ore", "Coal Ore", 14, 1, NamespacedKey.Base("coal"), 10);
                AddStandardBlock("wooden_planks", "Wooden Planks", 4, 1, NamespacedKey.Base("wooden_planks"), 4);
            */
        }
    }
}