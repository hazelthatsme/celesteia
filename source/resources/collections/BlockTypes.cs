using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Collections {
    public abstract class BlockSpriteProperties {
        public const int SIZE = 8; 
    }

    public class BlockTypes {        
        private List<BlockType> Types;

        private TextureAtlas _atlas;
        private readonly RectangleF standardBox = new RectangleF(0f, 0f, 1f, 1f);
        public BlockFrames BreakAnimation;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading block types...");

            BreakAnimation = new BlockFrames(TextureAtlas.Create("blockbreak",
                Content.Load<Texture2D>("sprites/blockbreak"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            ), 0, 3);

            Types = new List<BlockType>();

            _atlas = TextureAtlas.Create("blocks",
                Content.Load<Texture2D>("sprites/blocks"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            );

            AddBlock("Air", 0, 0);
            AddStandardBlock("Stone", 2, 1, ResourceManager.Items.GetItem("Stone"), 5);
            AddStandardBlock("Soil", 1, 1, ResourceManager.Items.GetItem("Soil"), 5);
            AddStandardBlock("Grown Soil", 0, 1, ResourceManager.Items.GetItem("Soil"), 5);
            AddStandardBlock("Deepstone", 3, 1, ResourceManager.Items.GetItem("Deepstone"), -1);
            AddStandardBlock("Wooden Log", 10, 1, ResourceManager.Items.GetItem("Wooden Log"), 3);
            AddBlock("Leaves", 11, 1);
            AddStandardBlock("Iron Ore", 8, 1, ResourceManager.Items.GetItem("Iron Ore"), 10);
            AddStandardBlock("Copper Ore", 7, 1, ResourceManager.Items.GetItem("Copper Ore"), 10);
            AddStandardBlock("Coal Ore", 14, 1, ResourceManager.Items.GetItem("Coal Lump"), 10);
            AddStandardBlock("Wooden Planks", 4, 1, ResourceManager.Items.GetItem("Wooden Planks"), 4);
        }

        byte next = 0;
        private void AddBlock(string name, int frameStart, int frameCount = 1, ItemType item = null, RectangleF? boundingBox = null, int strength = 1) {
            Types.Add(new BlockType(next, name, _atlas, frameStart, frameCount, item, boundingBox, strength));
            next++;
        }

        private void AddStandardBlock(string name, int frameStart, int frameCount = 1, ItemType item = null, int strength = 1) {
            AddBlock(name, frameStart, frameCount, item, standardBox, strength);
        }

        public BlockType GetBlock(byte id) {
            return Types.Find(x => x.BlockID == id);
        }
    }

    public class BlockType {
        public readonly byte BlockID;
        public readonly string Name;
        public readonly BlockFrames Frames;
        public readonly ItemType Item;
        public readonly RectangleF? BoundingBox;
        public readonly int Strength;

        public BlockType(byte id, string name, TextureAtlas atlas, int frameStart, int frameCount, ItemType item, RectangleF? boundingBox, int strength) {
            Debug.WriteLine($"  Loading block '{name}' ({id})...");

            BlockID = id;
            Name = name;
            Item = item;
            Frames = new BlockFrames(atlas, frameStart, frameCount);
            BoundingBox = boundingBox;
            Strength = strength;
        }
    }
}