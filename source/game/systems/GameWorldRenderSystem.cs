using System.Collections.Generic;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class GameWorldRenderSystem : UpdateSystem, IDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private ChunkVector _pivotChunkPos => ChunkVector.FromVector2(_camera.Center);
        private int _renderDistance => MathHelper.Clamp(7 - _camera.Zoom, 1, 7);
        private GameWorld _gameWorld;

        public GameWorldRenderSystem(Camera2D camera, SpriteBatch spriteBatch, GameWorld world) {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _gameWorld = world;
        }

        private List<ChunkVector> activeChunks = new List<ChunkVector>();
        private Queue<ChunkVector> toDeactivate = new Queue<ChunkVector>();

        private ChunkVector _dv;
        public void DisableNextChunk() {
            if (toDeactivate.Count > 0) {
                _dv = toDeactivate.Dequeue();
                _gameWorld.GetChunk(_dv).Enabled = false;
            }
        }

        private void EnableChunk(ChunkVector cv) {
            Chunk c = _gameWorld.GetChunk(cv);
            
            if (c != null) c.Enabled = true;
        }

        private void DisableChunk(ChunkVector cv) {
            Chunk c = _gameWorld.GetChunk(cv);
            
            if (c != null) c.Enabled = false;
        }

        private List<ChunkVector> toQueue;
        private ChunkVector _v;
        public void UpdateChunks(ChunkVector vector, int renderDistance) {
            toQueue = new List<ChunkVector>(activeChunks);
            activeChunks.Clear();

            int minX = vector.X - renderDistance;
            int maxX = vector.X + renderDistance;

            int minY = vector.Y - renderDistance;
            int maxY = vector.Y + renderDistance;
            
            for (int i = minX; i <= maxX; i++) {
                _v.X = i;
                for (int j = minY; j <= maxY; j++) {
                    _v.Y = j;

                    if (!_gameWorld.ChunkIsInWorld(_v)) continue;

                    activeChunks.Add(_v);
                    if (toQueue.Contains(_v)) toQueue.Remove(_v);
                }
            }

            toQueue.ForEach(cv => { if (!toDeactivate.Contains(cv)) toDeactivate.Enqueue(cv); });
            activeChunks.ForEach(cv => EnableChunk(cv));

            System.Diagnostics.Debug.WriteLine(activeChunks.Count);

            toQueue.Clear();
        }

        private ChunkVector _prevChunkPos = new ChunkVector(int.MinValue, int.MinValue);
        private int _prevRenderDistance = 0;
        public override void Update(GameTime gameTime) {
            if (_prevChunkPos != _pivotChunkPos || _prevRenderDistance != _renderDistance) {
                //UpdateChunks(_pivotChunkPos, _renderDistance);

                _prevRenderDistance = _renderDistance;
                _prevChunkPos = _pivotChunkPos;
            }

            DisableNextChunk();
        }

        public void Draw(GameTime gameTime) {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());

            int minX = _pivotChunkPos.X - _renderDistance;
            int maxX = _pivotChunkPos.X + _renderDistance;

            int minY = _pivotChunkPos.Y - _renderDistance;
            int maxY = _pivotChunkPos.Y + _renderDistance;
            
            for (int i = minX; i <= maxX; i++) {
                _v.X = i;
                for (int j = minY; j <= maxY; j++) {
                    _v.Y = j;

                    if (!_gameWorld.ChunkIsInWorld(_v)) continue;
                    
                    DrawChunk(_v, gameTime, _spriteBatch, _camera);
                    //if (toQueue.Contains(_v)) toQueue.Remove(_v);
                }
            }
            //activeChunks.ForEach(v => DrawChunk(v, gameTime, _spriteBatch, _camera));

            _spriteBatch.End();
        }

        private void DrawChunk(ChunkVector cv, GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            Chunk c = _gameWorld.GetChunk(cv);

            if (c != null) c.Draw(gameTime, spriteBatch, camera);
        }
    }
}