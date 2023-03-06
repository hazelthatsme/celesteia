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

    public class BlockTypes : IResourceCollection {        
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
            AddStandardBlock("stone", "Stone", 2, 1, NamespacedKey.Base("stone"), 5);
            AddStandardBlock("soil", "Soil", 1, 1, NamespacedKey.Base("soil"), 5);
            AddStandardBlock("grown_soil", "Grown Soil", 0, 1, NamespacedKey.Base("soil"), 5);
            AddStandardBlock("deepstone", "Deepstone", 3, 1, NamespacedKey.Base("deepstone"), -1);
            AddStandardBlock("log", "Wooden Log", 10, 1, NamespacedKey.Base("log"), 3);
            AddWalkthroughBlock("leaves", "Leaves", 11);
            AddStandardBlock("iron_ore", "Iron Ore", 8, 1, NamespacedKey.Base("iron_ore"), 10);
            AddStandardBlock("copper_ore", "Copper Ore", 7, 1, NamespacedKey.Base("copper_ore"), 10);
            AddStandardBlock("coal_ore", "Coal Ore", 14, 1, NamespacedKey.Base("coal"), 10);
            AddStandardBlock("wooden_planks", "Wooden Planks", 4, 1, NamespacedKey.Base("wooden_planks"), 4);

            BakedTypes = Types.ToArray();
        }

        private void AddKey(NamespacedKey key, byte id) {
            keys.Add(key.Qualify(), id);
            Debug.WriteLine($"  Loading block '{key.Qualify()}' ({id})...");
        }

        byte next = 0;
        private byte AddType(string name, int frameStart, int frameCount = 1, NamespacedKey? itemKey = null, RectangleF? boundingBox = null, int strength = 1) {
            Types.Add(new BlockType(next, name, _atlas, frameStart, frameCount, itemKey, boundingBox, strength));
            return next++;
        }

        private void AddEmptyBlock(string key, string name) {
            AddKey(NamespacedKey.Base(key), AddType(name, 0, 0));
        }

        private void AddStandardBlock(string key, string name, int frameStart, int frameCount = 1, NamespacedKey? itemKey = null, int strength = 1) {
            AddKey(NamespacedKey.Base(key), AddType(name, frameStart, frameCount, itemKey, standardBox, strength));
        }

        private void AddWalkthroughBlock(string key, string name, int frameStart, int frameCount = 1, NamespacedKey? itemKey = null, int strength = 1) {
            AddKey(NamespacedKey.Base(key), AddType(name, frameStart, frameCount, itemKey, null, strength));
        }

        public BlockType GetBlock(byte id) {
            return BakedTypes[id];
        }

        public IResourceType GetResource(NamespacedKey key) {
            if (!keys.ContainsKey(key.Qualify())) throw new NullReferenceException();
            return BakedTypes[keys[key.Qualify()]];
        }
    }

    public class BlockType : IResourceType {
        public readonly byte BlockID;
        public readonly string Name;
        public readonly BlockFrames Frames;
        public readonly NamespacedKey? DropKey;
        public readonly RectangleF? BoundingBox;
        public readonly int Strength;

        public BlockType(byte id, string name, TextureAtlas atlas, int frameStart, int frameCount, NamespacedKey? dropKey, RectangleF? boundingBox, int strength) {
            BlockID = id;
            Name = name;
            DropKey = dropKey;
            Frames = new BlockFrames(atlas, frameStart, frameCount);
            BoundingBox = boundingBox;
            Strength = strength;
        }

        public ItemType GetDrops() => DropKey.HasValue ? ResourceManager.Items.GetResource(DropKey.Value) as ItemType : null;

        public byte GetID() => BlockID;
    }
}