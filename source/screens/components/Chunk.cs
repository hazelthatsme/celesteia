using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Resources;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Sprites;
using Celesteia.Graphics;
using System;
using System.Diagnostics;
using Celesteia.World;

namespace Celesteia.Screens.Systems {
    public class Chunk {
        public const int CHUNK_SIZE = 16;

        public ChunkVector Position;
        private byte[,] tiles;

        public Chunk(ChunkVector vector) {
            Position = vector;
            tiles = new byte[CHUNK_SIZE, CHUNK_SIZE];
        }

        private int trueX, trueY;
        public void Generate(IWorldGenerator generator) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                trueX = (Position.X * CHUNK_SIZE) + i;
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    trueY = (Position.Y * CHUNK_SIZE) + j;
                    tiles[i, j] = generator.GetNaturalBlock(trueX, trueY);
                }
            }
        }

        private bool _enabled = false;
        public bool Enabled {
            get => _enabled;
            set => _enabled = value;
        }

        Vector2 v;
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                v.X = i;
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    v.Y = j;
                    ResourceManager.Blocks.GetBlock(tiles[i, j]).Frames.Draw(0, spriteBatch, camera.GetDrawingPosition(Position.Resolve() + v), Color.Gray);
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

        public ChunkVector(int x, int y) {
            X = x;
            Y = y;
        }

        public static ChunkVector FromVector2(Vector2 vector)
        {
            return new ChunkVector(
                (int)Math.Floor(vector.X / Chunk.CHUNK_SIZE),
                (int)Math.Floor(vector.Y / Chunk.CHUNK_SIZE)
            );
        }

        public Vector2 Resolve() {
            return new Vector2(X * Chunk.CHUNK_SIZE, Y * Chunk.CHUNK_SIZE);
        }

        public static int Distance(ChunkVector a, ChunkVector b) {
            return MathHelper.Max(Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
        }

        public static bool operator ==(ChunkVector a, ChunkVector b) {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(ChunkVector a, ChunkVector b) {
            return a.X != b.X || a.Y != b.Y;
        }
    }
}