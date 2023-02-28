using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Resources;
using Celesteia.Graphics;
using System;
using Celesteia.Game.Worlds.Generators;
using Celesteia.Resources.Collections;
using Celesteia.Resources.Sprites;

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
        private byte[,] wallTileMap;

        private GraphicsDevice _graphicsDevice;

        public Chunk(ChunkVector cv, GraphicsDevice graphicsDevice) {
            SetPosition(cv);
            _graphicsDevice = graphicsDevice;
            tileMap = new byte[CHUNK_SIZE, CHUNK_SIZE];
            wallTileMap = new byte[CHUNK_SIZE, CHUNK_SIZE];
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
                    byte[] natural = generator.GetNaturalBlocks(_truePosition.X + i, _truePosition.Y + j);
                    SetBlock(i, j, natural[0]);
                    SetWallBlock(i, j, natural[1]);
                }
            }
        }

        public byte GetBlock(int x, int y) {
            if (!IsInChunk(x, y)) return 0;

            return tileMap[x, y];
        }

        public void SetBlock(int x, int y, byte id) {
            if (!IsInChunk(x, y)) return;

            tileMap[x, y] = id;
        }

        public byte GetWallBlock(int x, int y) {
            if (!IsInChunk(x, y)) return 0;

            return wallTileMap[x, y];
        }

        public void SetWallBlock(int x, int y, byte id) {
            if (!IsInChunk(x, y)) return;

            wallTileMap[x, y] = id;
        }

        Vector2 v;
        BlockFrame frame;
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                v.X = i;
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    v.Y = j;
                    if (tileMap[i, j] == 0) {           // If the tile here is empty, draw the wall instead.
                        frame = ResourceManager.Blocks.GetBlock(wallTileMap[i, j]).Frames.GetFrame(0);
                        if (frame != null) DrawWallTile(i, j, frame, spriteBatch, camera);
                    }
                    else {                              // If there is a tile that isn't empty, draw the tile.
                        frame = ResourceManager.Blocks.GetBlock(tileMap[i, j]).Frames.GetFrame(0);
                        if (frame != null) DrawTile(i, j, frame, spriteBatch, camera);
                    }
                }
            }
        }

        public void DrawTile(int x, int y, BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            frame.Draw(0, spriteBatch, camera.GetDrawingPosition(_truePositionVector + v), Color.White, 0.4f);
        }

        public void DrawWallTile(int x, int y, BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            frame.Draw(0, spriteBatch, camera.GetDrawingPosition(_truePositionVector + v), Color.DarkGray, 0.5f);
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