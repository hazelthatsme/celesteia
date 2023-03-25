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

                    foreground[i, j].BlockID = natural[0];
                    background[i, j].BlockID = natural[1];
                }
            }
        }

        public byte GetForeground(int x, int y) => IsInChunk(x, y) ? foreground[x, y].BlockID : (byte)0;
        public byte GetBackground(int x, int y) => IsInChunk(x, y) ? foreground[x, y].BlockID : (byte)0;

        public void SetForeground(int x, int y, byte id) {
            if (IsInChunk(x, y)) {
                foreground[x, y].BlockID = id;
                foreground[x, y].BreakProgress = 0;
            }
        }

        public void SetBackground(int x, int y, byte id) {
            if (IsInChunk(x, y)) {
                background[x, y].BlockID = id;
                background[x, y].BreakProgress = 0;
            }
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

        private float progress;
        private byte state;

        private void DrawAllAt(int x, int y, GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            state = 0;

            if (foreground[x, y].DoDraw()) {
                state = 2;
                if (foreground[x, y].Type.Translucent && background[x, y].DoDraw()) state += 1;
            } else if (background[x, y].DoDraw()) state = 1;

            if (state == 0) return;

            if (state == 1 || state == 3) {
                progress = background[x, y].BreakProgress / (float) background[x, y].Type.Strength;

                DrawWallTile(x, y, background[x, y].Type.Frames.GetFrame(0), spriteBatch, camera);
                if (progress > 0f) DrawWallTile(x, y, ResourceManager.Blocks.BreakAnimation.GetProgressFrame(progress), spriteBatch, camera);
            }

            if (state == 2 || state == 3) {
                progress = foreground[x, y].BreakProgress / (float) foreground[x, y].Type.Strength;

                DrawTile(x, y, foreground[x, y].Type.Frames.GetFrame(0), spriteBatch, camera);
                if (progress > 0f) DrawTile(x, y, ResourceManager.Blocks.BreakAnimation.GetProgressFrame(progress), spriteBatch, camera);
            }
        }

        public void DrawTile(int x, int y, BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            if (frame == null) return;
            frame.Draw(0, spriteBatch, camera.GetDrawingPosition(_truePosition.X + x, _truePosition.Y + y), Color.White, 0.4f);
        }

        public void DrawWallTile(int x, int y, BlockFrame frame, SpriteBatch spriteBatch, Camera2D camera) {
            if (frame == null) return;
            frame.Draw(0, spriteBatch, camera.GetDrawingPosition(_truePosition.X + x, _truePosition.Y + y), Color.DarkGray, 0.5f);
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