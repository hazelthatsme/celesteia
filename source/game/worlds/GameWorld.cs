using System;
using System.Diagnostics;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Worlds.Generators;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Celesteia.Game.Worlds {
    public class GameWorld : IDisposable {
        private GameInstance _game;
        private GameInstance Game => _game;

        private Chunk[,] chunkMap;

        private int _width;
        public int GetWidth() => _width;
        public int GetWidthInBlocks() => _width * Chunk.CHUNK_SIZE;
        
        private int _height;
        public int GetHeight() => _height;
        public int GetHeightInBlocks() => _height * Chunk.CHUNK_SIZE;

        private int _seed;
        public int GetSeed() => _seed;

        public GameWorld(int width, int height, GameInstance game) {
            _game = game;
            
            _width = width;
            _height = height;

            _seed = (int) System.DateTime.Now.Ticks;

            chunkMap = new Chunk[width, height];
        }

        public GameWorld SetSeed(int seed) {
            _seed = seed;
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
            int count = 0;
            int total = _width * _height;
            for (int i = 0; i < _width; i++) {
                _gv.X = i;
                for (int j = 0; j < _height; j++) {
                    _gv.Y = j;

                    _c = new Chunk(_gv, Game.GraphicsDevice);
                    _c.Generate(_generator);

                    _c.Enabled = false;

                    count++;
                    if (progressReport != null) progressReport($"Generated {count}/{total} chunks.");

                    chunkMap[i, j] = _c;
                }
            }

            _generator.GenerateStructures();

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

        public byte GetBlock(Vector2 v) {
            return GetBlock(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y)
            );
        }


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

            if (ChunkIsInWorld(cv)) {
                GetChunk(cv).AddBreakProgress(x, y, power, wall, out drops);
                return true;
            } else return false;
        }

        public bool AddBreakProgress(Vector2 v, int power, bool wall, out ItemStack drops) {
            return AddBreakProgress(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y),
            power, wall, out drops);
        }

        public ChunkVector GetChunkVector(int x, int y) {
            ChunkVector cv = new ChunkVector(
                x / Chunk.CHUNK_SIZE,
                y / Chunk.CHUNK_SIZE
            );

            return cv;
        }

        public bool ChunkIsInWorld(ChunkVector cv) {
            return (cv.X >= 0 && cv.Y >= 0 && cv.X < _width && cv.Y < _height);
        }

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