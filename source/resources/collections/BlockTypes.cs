using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Collections {
    public abstract class BlockSpriteProperties {
        public const int SIZE = 8; 
    }

    public class BlockTypes {        
        private List<BlockType> Types;

        public void LoadContent(ContentManager Content) {
            Debug.WriteLine($"Loading block types...");

            Types = new List<BlockType>();

            TextureAtlas atlas = TextureAtlas.Create("blocks",
                Content.Load<Texture2D>("sprites/blocks"),
                BlockSpriteProperties.SIZE,
                BlockSpriteProperties.SIZE
            );

            Types.Add(new BlockType(0, "Air", null, 0, 0));

            Types.Add(new BlockType(1, "Stone", atlas, 2, 1));

            Types.Add(new BlockType(2, "Soil", atlas, 1, 1));

            Types.Add(new BlockType(3, "Grown Soil", atlas, 0, 1));

            Types.Add(new BlockType(4, "Deepstone", atlas, 3, 1));
        }

        public BlockType GetBlock(byte id) {
            return Types.Find(x => x.BlockID == id);
        }
    }

    public struct BlockType {
        public readonly byte BlockID;
        public readonly string Name;
        public readonly BlockFrames Frames;

        public BlockType(byte id, string name, TextureAtlas atlas, int frameStart, int frameCount) {
            BlockID = id;
            Name = name;
            Frames = new BlockFrames(atlas, BlockSpriteProperties.SIZE, frameStart, frameCount);

            Debug.WriteLine($"  Block '{name}' loaded.");
        }

        public BlockType(byte id, string name, TextureAtlas atlas, int frame) : this (id, name, atlas, frame, 1) {}
    }
}