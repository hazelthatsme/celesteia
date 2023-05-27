using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Resources;
using Celesteia.Graphics;
using System;
using Celesteia.Game.Worlds.Generators;
using Celesteia.Resources.Sprites;
using Celesteia.Game.Components.Items;
using Celesteia.Resources.Types;
using System.Diagnostics;
using System.Threading;

namespace Celesteia.Game.Worlds {
    public class Chunk {
        public const int CHUNK_SIZE = 16;
        public static bool IsInChunk(int x, int y) => x >= 0 && y >= 0 && y < CHUNK_SIZE && y < CHUNK_SIZE;

        public bool Enabled = false;
        public bool DoUpdate = false;

        private ChunkVector _position;
        private Point _truePosition;

        private BlockState[,] foreground;
        private BlockState[,] background;

        private GraphicsDevice _graphicsDevice;

        public Chunk(ChunkVector cv, GraphicsDevice graphicsDevice) {
            SetPosition(cv);
            _graphicsDevice = graphicsDevice;

            foreground = new BlockState[CHUNK_SIZE, CHUNK_SIZE];
            background = new BlockState[CHUNK_SIZE, CHUNK_SIZE];
        }

        public Chunk SetPosition(ChunkVector cv) {
            _position = cv;
            _truePosition = cv.Resolve();

            return this;
        }

        public void Generate(IWorldGenerator generator) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    byte[] natural = generator.GetNaturalBlocks(_truePosition.X + i, _truePosition.Y + j);

                    SetForeground(i, j, natural[0]);
                    SetBackground(i, j, natural[1]);
                }
            }

            DoUpdate = true;
        }

        public byte GetForeground(int x, int y) => IsInChunk(x, y) ? foreground[x, y].BlockID : (byte)0;
        public byte GetBackground(int x, int y) => IsInChunk(x, y) ? background[x, y].BlockID : (byte)0;

        public void SetForeground(int x, int y, byte id) {
            if (IsInChunk(x, y)) {
                foreground[x, y].BlockID = id;
                foreground[x, y].BreakProgress = 0;
                foreground[x, y].Draw = foreground[x, y].HasFrames();
                background[x, y].Draw = background[x, y].HasFrames() && foreground[x, y].Translucent;
            }
            DoUpdate = true;
        }

        public void SetBackground(int x, int y, byte id) {
            if (IsInChunk(x, y)) {
                background[x, y].BlockID = id;
                background[x, y].BreakProgress = 0;
                background[x, y].Draw = background[x, y].HasFrames() && foreground[x, y].Translucent;
            }
            DoUpdate = true;
        }

        private NamespacedKey? dropKey;
        public void AddBreakProgress(int x, int y, int power, bool wall, out ItemStack drops) {
            dropKey = null;
            drops = null;
            
            if (!IsInChunk(x, y)) return;

            if (wall) {
                background[x, y].BreakProgress += power;
                if (background[x, y].BreakProgress > background[x, y].Type.Strength) {
                    dropKey = background[x, y].Type.DropKey;
                    SetBackground(x, y, 0);
                }
            } else {
                foreground[x, y].BreakProgress += power;
                if (foreground[x, y].BreakProgress > foreground[x, y].Type.Strength) {
                    dropKey = foreground[x, y].Type.DropKey;
                    SetForeground(x, y, 0);
                }
            }

            if (dropKey.HasValue) drops = new ItemStack(dropKey.Value, 1);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            for (int i = 0; i < CHUNK_SIZE; i++) {
                for (int j = 0; j < CHUNK_SIZE; j++) {
                    DrawAllAt(i, j, gameTime, spriteBatch, camera);
                }
            }
        }

        Vector2 trueV = Vector2.Zero;
        private void DrawAllAt(int x, int y, GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            trueV.X = _truePosition.X + x;
            trueV.Y = _truePosition.Y + y;

            if (background[x, y].Draw) {
                DrawWallTile(background[x, y].Frames.GetFrame(0), spriteBatch, camera);
                if (background[x, y].BreakProgress > 0) DrawWallTile(ResourceManager.Blocks.BreakAnimation.GetProgressFrame(
                    // Background block breaking progress.
                    background[x, y].BreakProgress / (float) background[x, y].Type.Strength
                ), spriteBatch, camera);
            }
            if (foreground[x, y].Draw) {
                DrawTile(foreground[x, y].Frames.GetFrame(0), spriteBatch, camera);
                if (foreground[x, y].BreakProgress > 0) DrawTile(ResourceManager.Blocks.BreakAnimation.GetProgressFrame(
                    // Foreground block breaking progress.
                    foreground[x, y].BreakProgress / (float) foreground[x, y].Type.Strength
                ), spriteBatch, camera);
            }  
        }

        public void DrawTile(BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            if (frame == null) return;
            frame.Draw(0, spriteBatch, camera.GetDrawingPosition(trueV), Color.White, 0.4f);
        }

        public void DrawWallTile(BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            if (frame == null) return;
            frame.Draw(0, spriteBatch, trueV, Color.DarkGray, 0.5f);
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