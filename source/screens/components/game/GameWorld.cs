using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Graphics;
using Celesteia.Screens.Systems;
using Celesteia.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Screens.Components.Game {
    public class GameWorld : IDisposable {
        public Chunk[,] chunkMap;

        private List<ChunkVector> activeChunks = new List<ChunkVector>();
        private Queue<ChunkVector> toDeactivate = new Queue<ChunkVector>();

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
        public void Generate() {
            Chunk chunk;

            for (int i = 0; i < _width; i++) {
                _gv.X = i;
                for (int j = 0; j < _height; j++) {
                    _gv.Y = j;

                    chunk = new Chunk(_gv);
                    chunk.Generate(_generator);

                    chunk.Enabled = false;
                    chunkMap[i, j] = chunk;
                }
            }
        }

        private ChunkVector _dv;
        public void DisableNextChunk() {
            if (toDeactivate.Count > 0) {
                _dv = toDeactivate.Dequeue();
                SetChunkEnabled(_dv, false);
            }
        }

        private List<ChunkVector> toQueue;
        private ChunkVector _v;
        public void UpdateChunks(ChunkVector vector, int renderDistance) {
            toQueue = new List<ChunkVector>(activeChunks);
            activeChunks.Clear();

            int minX = Math.Max(0, vector.X - renderDistance);
            int maxX = Math.Min(_width - 1, vector.X + renderDistance);

            int minY = Math.Max(0, vector.Y - renderDistance);
            int maxY = Math.Min(_height - 1, vector.Y + renderDistance);

            Debug.WriteLine($"{minX} - {maxX}, {minY} - {maxY}");
            
            for (int i = minX; i <= maxX; i++) {
                _v.X = i;
                for (int j = minY; j <= maxY; j++) {
                    _v.Y = j;

                    activeChunks.Add(_v);
                    if (toQueue.Contains(_v)) toQueue.Remove(_v);
                }
            }

            toQueue.ForEach(v => { if (!toDeactivate.Contains(v)) toDeactivate.Enqueue(v); });
            activeChunks.ForEach(v => SetChunkEnabled(v, true));

            toQueue.Clear();
        }

        private void SetChunkEnabled(ChunkVector v, bool value) {
            if (chunkMap[v.X, v.Y] != null) chunkMap[v.X, v.Y].Enabled = value;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            Debug.WriteLine(activeChunks.Count);
            activeChunks.ForEach(v => DrawChunk(v, gameTime, spriteBatch, camera));
        }

        private void DrawChunk(ChunkVector v, GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            if (chunkMap[v.X, v.Y] != null) chunkMap[v.X, v.Y].Draw(gameTime, spriteBatch, camera);
        }

        public void Dispose() {
            chunkMap = null;
        }
    }
}