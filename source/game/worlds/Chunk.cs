using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Resources;
using Celesteia.Graphics;
using System;
using Celesteia.Game.Worlds.Generators;
using Celesteia.Resources.Sprites;
using Celesteia.Game.Components.Items;
using Celesteia.Resources.Types;

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

        public bool DoUpdate = false;

        private ChunkVector _position;
        private Point _truePosition;
        private Vector2 _truePositionVector;
        private byte[,] tileMap;
        private byte[,] wallTileMap;
        private int[,] tileBreakProgressMap;
        private int[,] wallTileBreakProgressMap;
        private byte[,] drawState;

        private GraphicsDevice _graphicsDevice;

        public Chunk(ChunkVector cv, GraphicsDevice graphicsDevice) {
            SetPosition(cv);
            _graphicsDevice = graphicsDevice;
            tileMap = new byte[CHUNK_SIZE, CHUNK_SIZE];
            wallTileMap = new byte[CHUNK_SIZE, CHUNK_SIZE];
            tileBreakProgressMap = new int[CHUNK_SIZE, CHUNK_SIZE];
            wallTileBreakProgressMap = new int[CHUNK_SIZE, CHUNK_SIZE];
            drawState = new byte[CHUNK_SIZE, CHUNK_SIZE];
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
            tileBreakProgressMap[x, y] = 0;
        }

        public byte GetWallBlock(int x, int y) {
            if (!IsInChunk(x, y)) return 0;

            return wallTileMap[x, y];
        }

        public void SetWallBlock(int x, int y, byte id) {
            if (!IsInChunk(x, y)) return;

            wallTileMap[x, y] = id;
            wallTileBreakProgressMap[x, y] = 0;
        }

        private NamespacedKey? dropKey;
        public void AddBreakProgress(int x, int y, int power, bool wall, out ItemStack drops) {
            dropKey = null;
            drops = null;
            if (!IsInChunk(x, y)) return;

            if (wall) {
                wallTileBreakProgressMap[x, y] += power;
                if (wallTileBreakProgressMap[x, y] > ResourceManager.Blocks.GetBlock(wallTileMap[x, y]).Strength) {
                    dropKey = ResourceManager.Blocks.GetBlock(wallTileMap[x, y]).DropKey;
                    SetWallBlock(x, y, 0);
                }
            } else {
                tileBreakProgressMap[x, y] += power;
                if (tileBreakProgressMap[x, y] > ResourceManager.Blocks.GetBlock(tileMap[x, y]).Strength) {
                    dropKey = ResourceManager.Blocks.GetBlock(tileMap[x, y]).DropKey;
                    SetBlock(x, y, 0);
                }
            }

            if (dropKey.HasValue) drops = new ItemStack(dropKey.Value, 1);
        }

        // DRAW STATES
        // 0: draw nothing
        // 1: draw back
        // 2: draw front
        // 3: draw both
        public void Update(GameTime gameTime) {
        }

        Vector2 v;
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                v.X = i;
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    v.Y = j;
                    DrawAllAt(i, j, gameTime, spriteBatch, camera);
                }
            }
        }

        private BlockType front;
        private BlockType back;
        private float progress;
        private byte state;

        private void DrawAllAt(int x, int y, GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            state = 0;

            front = ResourceManager.Blocks.GetBlock(tileMap[x, y]);
            back = ResourceManager.Blocks.GetBlock(wallTileMap[x, y]);

            if (front.Frames != null) {
                state = 2;
                if (front.Translucent && back.Frames != null) state += 1;
            } else if (back.Frames != null) state = 1;

            if (state == 0) return;

            if (state == 1 || state == 3) {
                progress = ((float)wallTileBreakProgressMap[x, y] / (float)back.Strength);

                DrawWallTile(x, y, back.Frames.GetFrame(0), spriteBatch, camera);
                if (progress > 0f) DrawWallTile(x, y, ResourceManager.Blocks.BreakAnimation.GetProgressFrame(progress), spriteBatch, camera);
            }

            if (state == 2 || state == 3) {
                progress = ((float)tileBreakProgressMap[x, y] / (float)front.Strength);

                DrawTile(x, y, front.Frames.GetFrame(0), spriteBatch, camera);
                if (progress > 0f) DrawTile(x, y, ResourceManager.Blocks.BreakAnimation.GetProgressFrame(progress), spriteBatch, camera);
            }
        }

        public void DrawTile(int x, int y, BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            if (frame == null) return;
            frame.Draw(0, spriteBatch, camera.GetDrawingPosition(_truePositionVector.X + x, _truePositionVector.Y + y), Color.White, 0.4f);
        }

        public void DrawWallTile(int x, int y, BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            if (frame == null) return;
            frame.Draw(0, spriteBatch, camera.GetDrawingPosition(_truePositionVector.X + x, _truePositionVector.Y + y), Color.DarkGray, 0.5f);
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
            return a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj is ChunkVector) return ((ChunkVector)obj).X == this.X && ((ChunkVector)obj).Y == this.Y;
            return false;
        }

        public static bool operator !=(ChunkVector a, ChunkVector b) {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}