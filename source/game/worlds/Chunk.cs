using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Resources;
using Celesteia.Graphics;
using System;
using Celesteia.Game.Worlds.Generators;

namespace Celesteia.Game.Worlds {
    public class Chunk {
        public const int CHUNK_SIZE = 16;
        public static bool IsInChunk(int x, int y) {
            return (x >= 0 && y >= 0 && y < CHUNK_SIZE && y < CHUNK_SIZE);
        }

        private bool _enabled = false;
        public bool Enabled {
            get => _enabled;
            set => _enabled = value;
        }

        private ChunkVector _position;
        private Point _truePosition;
        private Vector2 _truePositionVector;
        private byte[,] tileMap;

        public Chunk(ChunkVector cv) {
            SetPosition(cv);
            tileMap = new byte[CHUNK_SIZE, CHUNK_SIZE];
        }

        public Chunk SetPosition(ChunkVector cv) {
            _position = cv;
            _truePosition = cv.Resolve();
            _truePositionVector = new Vector2(_truePosition.X, _truePosition.Y);

            return this;
        }

        public void Generate(IWorldGenerator generator) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    SetBlock(i, j, generator.GetNaturalBlock(_truePosition.X + i, _truePosition.Y + j));
                }
            }
        }

        public void SetBlock(int x, int y, byte id) {
            if (!IsInChunk(x, y)) return;

            tileMap[x, y] = id;
        }

        Vector2 v;
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                v.X = i;
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    v.Y = j;
                    ResourceManager.Blocks.GetBlock(tileMap[i, j]).Frames.Draw(0, spriteBatch, camera.GetDrawingPosition(_truePositionVector + v), Color.Gray);
                }
            }
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

        public Point Resolve() {
            return new Point(X * Chunk.CHUNK_SIZE, Y * Chunk.CHUNK_SIZE);
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