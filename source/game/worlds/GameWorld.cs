using System;
using System.Diagnostics;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Worlds.Generators;
using Celesteia.Resources;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Celesteia.Game.Worlds {
    public class GameWorld : IDisposable {
        private GameInstance Game;

        private Chunk[,] chunkMap;

        public int Width { get; private set; }
        public int BlockWidth => Width * Chunk.CHUNK_SIZE;

        public int Height { get; private set; }
        public int BlockHeight => Height * Chunk.CHUNK_SIZE;

        public int Seed { get; private set; }

        private Vector2? _selection;
        private BlockType _selectedBlock;
        public Vector2? GetSelection() => _selection;
        public BlockType GetSelectedBlock() => _selectedBlock;
        public void SetSelection(Vector2? selection) {
            _selection = selection;
            if (!_selection.HasValue) return;

            _selection = RoundSelection(_selection.Value);
            _selectedBlock = ResourceManager.Blocks.GetBlock(GetBlock(_selection.Value));
        }
        private Vector2 RoundSelection(Vector2 pos) {
            pos.X = (int)Math.Floor(pos.X);
            pos.Y = (int)Math.Floor(pos.Y);
            return pos;
        }

        public GameWorld(int width, int height, GameInstance game) {
            Game = game;
            
            Width = width;
            Height = height;

            Seed = (int) System.DateTime.Now.Ticks;

            chunkMap = new Chunk[width, height];
        }

        public GameWorld SetSeed(int seed) {
            Seed = seed;
            return this;
        }

        private IWorldGenerator _generator;
        public GameWorld SetGenerator(IWorldGenerator generator) {
            _generator = generator;
            return this;
        }
        public IWorldGenerator GetGenerator() {
            return _generator;
        }

        private ChunkVector _gv;
        private Chunk _c;
        public void Generate(Action<string> progressReport = null) {
            if (progressReport != null) progressReport("Generating chunks...");
            for (int i = 0; i < Width; i++) {
                _gv.X = i;
                for (int j = 0; j < Height; j++) {
                    _gv.Y = j;

                    _c = new Chunk(_gv, Game.GraphicsDevice);
                    _c.Generate(_generator);

                    _c.Enabled = false;

                    chunkMap[i, j] = _c;
                }
            }

            if (progressReport != null) progressReport("Generating structures...");
            _generator.GenerateStructures(progressReport);

            Debug.WriteLine("World generated.");
        }

        public Chunk GetChunk(ChunkVector cv) {
            return chunkMap[cv.X, cv.Y];
        }


        public byte GetBlock(int x, int y) {
            ChunkVector cv = GetChunkVector(x, y);
            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (ChunkIsInWorld(cv)) return GetChunk(cv).GetBlock(x, y);
            else return 0;
        }
        public byte GetBlock(Point pos) => GetBlock(pos.X, pos.Y);
        public byte GetBlock(Vector2 v) => GetBlock(v.ToPoint());

        public byte GetWallBlock(int x, int y) {
            ChunkVector cv = GetChunkVector(x, y);
            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (ChunkIsInWorld(cv)) return GetChunk(cv).GetWallBlock(x, y);
            else return 0;
        }

        public byte GetWallBlock(Vector2 v) {
            return GetWallBlock(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y)
            );
        }


        public bool GetAnyBlock(int x, int y, bool includeWalls) {
            ChunkVector cv = GetChunkVector(x, y);
            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (!ChunkIsInWorld(cv)) return false;

            return (includeWalls && GetChunk(cv).GetWallBlock(x, y) != 0) || GetChunk(cv).GetBlock(x, y) != 0;
        }

        public bool GetAnyBlock(Vector2 v, bool includeWalls) {
            return GetAnyBlock(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y),
                includeWalls
            );
        }

        public void SetBlock(int x, int y, byte id) {
            ChunkVector cv = GetChunkVector(x, y);
            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (ChunkIsInWorld(cv)) GetChunk(cv).SetBlock(x, y, id);
        }

        public void SetBlock(Vector2 v, byte id) {
            SetBlock(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y),
                id
            );
        }

        public void SetWallBlock(int x, int y, byte id) {
            ChunkVector cv = GetChunkVector(x, y);
            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (ChunkIsInWorld(cv)) GetChunk(cv).SetWallBlock(x, y, id);
        }

        public void SetWallBlock(Vector2 v, byte id) {
            SetWallBlock(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y),
                id
            );
        }
        public void RemoveBlock(Vector2 v) {
            RemoveBlock(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y)
            );
        }

        public void RemoveBlock(int x, int y) {
            SetBlock(x, y, 0);
        }

        public bool AddBreakProgress(int x, int y, int power, bool wall, out ItemStack drops) {
            ChunkVector cv = GetChunkVector(x, y);
            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            drops = null;

            if (!ChunkIsInWorld(cv)) return false;

            GetChunk(cv).AddBreakProgress(x, y, power, wall, out drops);
            return true;
        }

        public bool AddBreakProgress(Vector2 v, int power, bool wall, out ItemStack drops) {
            return AddBreakProgress(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y),
            power, wall, out drops);
        }

        private ChunkVector GetChunkVector(int x, int y) => new ChunkVector(x / Chunk.CHUNK_SIZE, y / Chunk.CHUNK_SIZE);
        public bool ChunkIsInWorld(ChunkVector cv) => cv.X >= 0 && cv.Y >= 0 && cv.X < Width && cv.Y < Height;

        public RectangleF? GetBlockBoundingBox(int x, int y) {
            return TestBoundingBox(x, y, GetBlock(x, y));
        }

        public RectangleF? TestBoundingBox(int x, int y, byte id) {
            RectangleF? box = ResourceManager.Blocks.GetBlock(id).BoundingBox;

            if (!box.HasValue) return null;
            return new RectangleF(
                x, y,
                box.Value.Width, box.Value.Height
            );
        }

        public RectangleF? TestBoundingBox(Vector2 v, byte id) {
            return TestBoundingBox(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y),
                id
            );
        }

        public Vector2 GetSpawnpoint() {
            return _generator.GetSpawnpoint();
        }

        public void Dispose() {
            chunkMap = null;
        }
    }
}