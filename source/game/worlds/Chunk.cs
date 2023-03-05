using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Resources;
using Celesteia.Graphics;
using System;
using Celesteia.Game.Worlds.Generators;
using Celesteia.Resources.Collections;
using Celesteia.Resources.Sprites;
using Celesteia.Game.Components.Items;

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
        private int[,] tileBreakProgressMap;
        private int[,] wallTileBreakProgressMap;

        private GraphicsDevice _graphicsDevice;

        public Chunk(ChunkVector cv, GraphicsDevice graphicsDevice) {
            SetPosition(cv);
            _graphicsDevice = graphicsDevice;
            tileMap = new byte[CHUNK_SIZE, CHUNK_SIZE];
            wallTileMap = new byte[CHUNK_SIZE, CHUNK_SIZE];
            tileBreakProgressMap = new int[CHUNK_SIZE, CHUNK_SIZE];
            wallTileBreakProgressMap = new int[CHUNK_SIZE, CHUNK_SIZE];
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

        public void AddBreakProgress(int x, int y, int power, bool wall, out ItemStack drops) {
            drops = null;
            if (!IsInChunk(x, y)) return;

            if (wall) {
                wallTileBreakProgressMap[x, y] += power;
                if (wallTileBreakProgressMap[x, y] > ResourceManager.Blocks.GetBlock(wallTileMap[x, y]).Strength) {
                    if (ResourceManager.Blocks.GetBlock(wallTileMap[x, y]).Item != null)
                        drops = new ItemStack(ResourceManager.Blocks.GetBlock(wallTileMap[x, y]).Item.ItemID, 1);
                    SetWallBlock(x, y, 0);
                }
            } else {
                tileBreakProgressMap[x, y] += power;
                if (tileBreakProgressMap[x, y] > ResourceManager.Blocks.GetBlock(tileMap[x, y]).Strength) {
                    if (ResourceManager.Blocks.GetBlock(tileMap[x, y]).Item != null)
                        drops = new ItemStack(ResourceManager.Blocks.GetBlock(tileMap[x, y]).Item.ItemID, 1);
                    SetBlock(x, y, 0);
                }
            }
        }

        Vector2 v;
        bool wall;
        BlockType type;
        BlockFrame frame;
        float breakProgress;
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                v.X = i;
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    v.Y = j;
                    wall = tileMap[i, j] == 0;
                    type = wall ? ResourceManager.Blocks.GetBlock(wallTileMap[i, j]) : ResourceManager.Blocks.GetBlock(tileMap[i, j]);
                    
                    frame = type.Frames.GetFrame(0);
                    if (frame == null) continue;

                    breakProgress = wall ? ((float)wallTileBreakProgressMap[i, j] / (float)type.Strength) : ((float)tileBreakProgressMap[i, j] / (float)type.Strength);

                    if (wall) {             // If the tile here is empty, draw the wall instead.
                        DrawWallTile(i, j, frame, spriteBatch, camera);
                        if (breakProgress > 0f) DrawWallTile(i, j, ResourceManager.Blocks.BreakAnimation.GetProgressFrame(breakProgress), spriteBatch, camera);
                    }
                    else {                  // If there is a tile that isn't empty, draw the tile.
                        DrawTile(i, j, frame, spriteBatch, camera);
                        if (breakProgress > 0f) DrawTile(i, j, ResourceManager.Blocks.BreakAnimation.GetProgressFrame(breakProgress), spriteBatch, camera);
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