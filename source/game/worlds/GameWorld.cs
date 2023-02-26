using System;
using Celesteia.Game.Worlds.Generators;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Worlds {
    public class GameWorld : IDisposable {
        private Chunk[,] chunkMap;

        private int _width;
        public int GetWidth() => _width;
        
        private int _height;
        public int GetHeight() => _height;

        public GameWorld(int width, int height) {
            _width = width;
            _height = height;

            chunkMap = new Chunk[width, height];
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
        }


        public Chunk GetChunk(ChunkVector cv) {
            return chunkMap[cv.X, cv.Y];
        }

        public void RemoveBlock(Vector2 v) {
            RemoveBlock(
                (int)Math.Floor(v.X),
                (int)Math.Floor(v.Y)
            );
        }

        public void RemoveBlock(int x, int y) {
            ChunkVector cv = new ChunkVector(
                x / Chunk.CHUNK_SIZE,
                y / Chunk.CHUNK_SIZE
            );

            x %= Chunk.CHUNK_SIZE;
            y %= Chunk.CHUNK_SIZE;

            if (ChunkIsInWorld(cv)) GetChunk(cv).SetBlock(x, y, 0);
        }

        public bool ChunkIsInWorld(ChunkVector cv) {
            return (cv.X >= 0 && cv.Y >= 0 && cv.X < _width && cv.Y < _height);
        }

        public void Dispose() {
            chunkMap = null;
        }
    }
}