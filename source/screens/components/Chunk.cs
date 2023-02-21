using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Resources;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Sprites;

namespace Celesteia.Screens.Systems {
    public class Chunk {
        public const int CHUNK_SIZE = 16;

        public ChunkVector Position;
        public byte[,] tiles;

        public Chunk() {
            tiles = new byte[CHUNK_SIZE, CHUNK_SIZE];
        }

        Vector2 v;
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                v.X = i;
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    v.Y = j;
                    ResourceManager.Blocks.GetBlock(tiles[i, j]).Frames.Draw(0, spriteBatch, Position.Resolve() + v, Color.White);
                }
            }
        }

        public static void DrawTile(
            SpriteBatch spriteBatch,
            TextureRegion2D frame,
            Vector2 position,
            Color color,
            float scale,
            SpriteEffects effects
        ) => DrawTile(spriteBatch, frame, position, color, 0f, Vector2.Zero, new Vector2(scale), effects, 0f);

        public static void DrawTile(
            SpriteBatch spriteBatch,
            TextureRegion2D textureRegion,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth,
            Rectangle? clippingRectangle = null) {
                spriteBatch.Draw(textureRegion, position, color, rotation, origin, scale, effects, layerDepth, clippingRectangle);
            }
    }

    public struct ChunkVector {
        public int X;
        public int Y;

        public Vector2 Resolve() {
            return new Vector2(X * Chunk.CHUNK_SIZE, Y * Chunk.CHUNK_SIZE);
        }
    }
}