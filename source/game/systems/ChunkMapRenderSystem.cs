using System.Collections.Generic;
using Celesteia.Game.Planets;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class ChunkMapRenderSystem : IUpdateSystem, IDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private ChunkVector _lastChunkPos;
        private ChunkVector _pivotChunkPos => ChunkVector.FromVector2(_camera.Center);
        public int RenderDistance = 5;
        private ChunkMap _chunkMap;

        public ChunkMapRenderSystem(Camera2D camera, SpriteBatch spriteBatch, ChunkMap chunkMap) {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _chunkMap = chunkMap;
        }

        public void Initialize(MonoGame.Extended.Entities.World world) {}

        private ChunkVector _v;
        private List<ChunkVector> activeChunks = new List<ChunkVector>();
        public void Update(GameTime gameTime)
        {
            if (_lastChunkPos != _pivotChunkPos) {
                activeChunks.Clear();
                for (int i = -RenderDistance; i < RenderDistance; i++) {
                    _v.X = _pivotChunkPos.X + i;
                    for (int j = -RenderDistance; j < RenderDistance; j++) {
                        _v.Y = _pivotChunkPos.Y + j;

                        if (!_chunkMap.ChunkIsInMap(_v)) continue;
                        activeChunks.Add(_v);
                    }
                }

                _lastChunkPos = _pivotChunkPos;
            }
        }

        public void Draw(GameTime gameTime) {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null, null, _camera.GetViewMatrix());

            // Draw every chunk in view.
            foreach (ChunkVector cv in activeChunks) DrawChunk(cv, gameTime, _spriteBatch);

            _spriteBatch.End();
        }

        private void DrawChunk(ChunkVector cv, GameTime gameTime, SpriteBatch spriteBatch) {
            Chunk c = _chunkMap.GetChunk(cv);

            if (c != null) c.Draw(gameTime, spriteBatch);
        }

        public void Dispose() {}
    }
}