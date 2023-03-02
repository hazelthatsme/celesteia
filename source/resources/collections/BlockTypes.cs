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

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading block types...");

            Types = new List<BlockType>();

            _atlas = TextureAtlas.Create("blocks",
                Content.Load<Texture2D>("sprites/blocks"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            );

            AddType("Air", 0, 0);
            AddStandardBlock("Stone", 2, 1, ResourceManager.Items.GetItem("Stone"));
            AddStandardBlock("Soil", 1, 1, ResourceManager.Items.GetItem("Soil"));
            AddStandardBlock("Grown Soil", 0, 1, ResourceManager.Items.GetItem("Soil"));
            AddStandardBlock("Deepstone", 3, 1, ResourceManager.Items.GetItem("Deepstone"));
            AddStandardBlock("Wooden Log", 10, 1, ResourceManager.Items.GetItem("Wooden Log"));
            AddType("Leaves", 11, 1);
            AddStandardBlock("Iron Ore", 8, 1, ResourceManager.Items.GetItem("Iron Ore"));
            AddStandardBlock("Copper Ore", 7, 1, ResourceManager.Items.GetItem("Copper Ore"));
            AddStandardBlock("Coal Ore", 14, 1, ResourceManager.Items.GetItem("Coal Lump"));
        }

        byte next = 0;
        private void AddType(string name, int frameStart, int frameCount = 1, ItemType item = null, RectangleF? boundingBox = null) {
            Types.Add(new BlockType(next, name, _atlas, frameStart, frameCount, item, boundingBox));
            next++;
        }

        private void AddStandardBlock(string name, int frameStart, int frameCount = 1, ItemType item = null) {
            AddType(name, frameStart, frameCount, item, standardBox);
        }

        public BlockType GetBlock(byte id) {
            return Types.Find(x => x.BlockID == id);
        }
    }

    public struct BlockType {
        public readonly byte BlockID;
        public readonly string Name;
        public readonly BlockFrames Frames;
        public readonly ItemType Item;
        public readonly RectangleF? BoundingBox;

        public BlockType(byte id, string name, TextureAtlas atlas, int frameStart, int frameCount = 1, ItemType item = null, RectangleF? boundingBox = null) {
            Debug.WriteLine($"  Loading block '{name}' ({id})...");

            BlockID = id;
            Name = name;
            Item = item;
            Frames = new BlockFrames(atlas, BlockSpriteProperties.SIZE, frameStart, frameCount);
            BoundingBox = boundingBox;
        }
    }
}