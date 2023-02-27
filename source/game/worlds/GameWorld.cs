using System;
using System.Diagnostics;
using Celesteia.Game.Worlds.Generators;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Worlds {
    public class GameWorld : IDisposable {
        private Chunk[,] chunkMap;

        private int _width;
        public int GetWidth() => _width;
        public int GetWidthInBlocks() => _width * Chunk.CHUNK_SIZE;
        
        private int _height;
        public int GetHeight() => _height;
        public int GetHeightInBlocks() => _height * Chunk.CHUNK_SIZE;

        private int _seed;
        public int GetSeed() => _seed;

        public GameWorld(int width, int height) {
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

        private ChunkVector _gv;
        private Chunk _c;
        public void Generate() {
            for (int i = 0; i < _width; i++) {
                _gv.X = i;
                for (int j = 0; j < _height; j++) {
                    _gv.Y = j;

                    _c = new Chunk(_gv);
                    _c.Generate(_generator);

                    _c.Enabled = false;
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
            ChunkVector cv = new ChunkVector(
                x / Chunk.CHUNK_SIZE,
                y / Chunk.CHUNK_SIZE
            );

            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (ChunkIsInWorld(cv)) return GetChunk(cv).GetBlock(x, y);
            else return 0;
        }

        public void SetBlock(int x, int y, byte id) {
            ChunkVector cv = new ChunkVector(
                x / Chunk.CHUNK_SIZE,
                y / Chunk.CHUNK_SIZE
            );

            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (ChunkIsInWorld(cv)) GetChunk(cv).SetBlock(x, y, id);
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

        public bool ChunkIsInWorld(ChunkVector cv) {
            return (cv.X >= 0 && cv.Y >= 0 && cv.X < _width && cv.Y < _height);
        }

        public Vector2 GetSpawnpoint() {
            return _generator.GetSpawnpoint();
        }

        public void Dispose() {
            chunkMap = null;
        }
    }
}