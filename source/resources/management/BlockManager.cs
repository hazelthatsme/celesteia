using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Sprites;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Management {
    public abstract class BlockSpriteProperties {
        public const int SIZE = 8; 
    }

    public class BlockManager : IResourceManager {        
        private List<BlockType> Types;
        private BlockType[] BakedTypes;
        private Dictionary<string, byte> keys = new Dictionary<string, byte>();

        public BlockFrames BreakAnimation;
        public BlockFrames Selection;

        private void LoadBreakingAnimation(ContentManager Content) {
            Debug.WriteLine($"Loading block break animation...");
            BreakAnimation = new BlockFrames(TextureAtlas.Create("blockbreak", Content.Load<Texture2D>("sprites/blockbreak"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            ), 0, 3);
        }

        private void LoadSelector(ContentManager Content) {
            Debug.WriteLine($"Loading block selector...");
            Selection = new BlockFrames(TextureAtlas.Create("selection", Content.Load<Texture2D>("sprites/blockselection"), 
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            ), 0, 1);
        }

        private List<IResourceCollection> _collections = new List<IResourceCollection>();
        public void AddCollection(IResourceCollection collection) => _collections.Add(collection);

        public void LoadContent(ContentManager Content) {
            LoadBreakingAnimation(Content);
            LoadSelector(Content);

            Debug.WriteLine($"Loading block types...");

            Types = new List<BlockType>();

            foreach (IResourceCollection collection in _collections)
                LoadCollection(collection);

            BakedTypes = Types.ToArray();
        }

        private void LoadCollection(IResourceCollection collection) {
            foreach (NamespacedKey key in collection.GetBlocks().Keys) {
                AddType(key, collection.GetBlocks()[key]);
            }
        }

        private byte next = 0;
        private void AddType(NamespacedKey key, BlockType type) {
            type.SetID(next++);
            keys.Add(key.Qualify(), type.GetID());

            Types.Add(type);
        }

        public BlockType GetBlock(byte id) => BakedTypes[id];

        public IResourceType GetResource(NamespacedKey key) {
            if (!keys.ContainsKey(key.Qualify())) throw new NullReferenceException();
            return BakedTypes[keys[key.Qualify()]];
        }
    }
}