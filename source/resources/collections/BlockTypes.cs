using System;
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
        private BlockType[] BakedTypes;
        private Dictionary<string, byte> keys = new Dictionary<string, byte>();

        private TextureAtlas _atlas;
        private readonly RectangleF standardBox = new RectangleF(0f, 0f, 1f, 1f);
        public BlockFrames BreakAnimation;
        public BlockFrames Selection;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading block types...");

            BreakAnimation = new BlockFrames(TextureAtlas.Create("blockbreak",
                Content.Load<Texture2D>("sprites/blockbreak"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            ), 0, 3);

            Selection = new BlockFrames(TextureAtlas.Create("selection",
                Content.Load<Texture2D>("sprites/blockselection"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            ), 0, 1);

            Types = new List<BlockType>();

            _atlas = TextureAtlas.Create("blocks",
                Content.Load<Texture2D>("sprites/blocks"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            );

            AddEmptyBlock("air", "Air");
            AddStandardBlock("stone", "Stone", 2, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("stone")), 5);
            AddStandardBlock("soil", "Soil", 1, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("soil")), 5);
            AddStandardBlock("grown_soil", "Grown Soil", 0, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("soil")), 5);
            AddStandardBlock("deepstone", "Deepstone", 3, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("deepstone")), -1);
            AddStandardBlock("log", "Wooden Log", 10, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("log")), 3);
            AddWalkthroughBlock("leaves", "Leaves", 11);
            AddStandardBlock("iron_ore", "Iron Ore", 8, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("iron_ore")), 10);
            AddStandardBlock("copper_ore", "Copper Ore", 7, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("copper_ore")), 10);
            AddStandardBlock("coal_ore", "Coal Ore", 14, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("coal")), 10);
            AddStandardBlock("wooden_planks", "Wooden Planks", 4, 1, ResourceManager.Items.GetItem(NamespacedKey.Base("wooden_planks")), 4);

            BakedTypes = Types.ToArray();
        }

        private void AddKey(NamespacedKey key, byte id) {
            keys.Add(key.Qualify(), id);
            Debug.WriteLine($"  Loading block '{key.Qualify()}' ({id})...");
        }

        byte next = 0;
        private byte AddBlock(string name, int frameStart, int frameCount = 1, ItemType item = null, RectangleF? boundingBox = null, int strength = 1) {
            Types.Add(new BlockType(next, name, _atlas, frameStart, frameCount, item, boundingBox, strength));
            return next++;
        }

        private void AddEmptyBlock(string key, string name) {
            AddKey(NamespacedKey.Base(key), AddBlock(name, 0, 0));
        }

        private void AddStandardBlock(string key, string name, int frameStart, int frameCount = 1, ItemType item = null, int strength = 1) {
            AddKey(NamespacedKey.Base(key), AddBlock(name, frameStart, frameCount, item, standardBox, strength));
        }

        private void AddWalkthroughBlock(string key, string name, int frameStart, int frameCount = 1, ItemType item = null, int strength = 1) {
            AddKey(NamespacedKey.Base(key), AddBlock(name, frameStart, frameCount, item, null, strength));
        }

        public BlockType GetBlock(byte id) {
            return BakedTypes[id];
        }

        public BlockType GetBlock(NamespacedKey key) {
            if (!keys.ContainsKey(key.Qualify())) throw new NullReferenceException();
            return BakedTypes[keys[key.Qualify()]];
        }
    }

    public class BlockType : IResourceType {
        public readonly byte BlockID;
        public readonly string Name;
        public readonly BlockFrames Frames;
        public readonly ItemType Item;
        public readonly RectangleF? BoundingBox;
        public readonly int Strength;

        public BlockType(byte id, string name, TextureAtlas atlas, int frameStart, int frameCount, ItemType item, RectangleF? boundingBox, int strength) {
            BlockID = id;
            Name = name;
            Item = item;
            Frames = new BlockFrames(atlas, frameStart, frameCount);
            BoundingBox = boundingBox;
            Strength = strength;
        }

        public byte GetID() {
            return BlockID;
        }
    }
}