using System.Collections.Generic;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Celesteia.Resources;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class GameWorldRenderSystem : IUpdateSystem, IDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private ChunkVector _lastChunkPos;
        private ChunkVector _pivotChunkPos => ChunkVector.FromVector2(_camera.Center);
        public int RenderDistance => 5;
        private GameWorld _gameWorld;
        private BlockFrame _selectionSprite;

        public GameWorldRenderSystem(Camera2D camera, SpriteBatch spriteBatch, GameWorld world) {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _gameWorld = world;

            _selectionSprite = ResourceManager.Blocks.Selection.GetFrame(0);
        }

        public void Initialize(World world) {}

        private ChunkVector _v;
        private List<ChunkVector> activeChunks = new List<ChunkVector>();
        private Color selectionColor;
        private bool drawSelection = false;
        public void Update(GameTime gameTime)
        {
            if (_lastChunkPos != _pivotChunkPos) {
                activeChunks.Clear();
                for (int i = -RenderDistance; i <= RenderDistance; i++) {
                    _v.X = _pivotChunkPos.X + i;
                    for (int j = -RenderDistance; j <= RenderDistance; j++) {
                        _v.Y = _pivotChunkPos.Y + j;

                        if (!_gameWorld.ChunkIsInWorld(_v)) continue;
                        _gameWorld.GetChunk(_v).DoUpdate = true;
                        activeChunks.Add(_v);
                    }
                }

                _lastChunkPos = _pivotChunkPos;
            }

            //foreach (ChunkVector cv in activeChunks) _gameWorld.GetChunk(_v).Update(gameTime);
            
            if (_gameWorld.GetSelection().HasValue) selectionColor = _gameWorld.GetSelectedBlock().Strength >= 0 ? Color.White : Color.Black;
        }

        public void Draw(GameTime gameTime) {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());

            // Draw every chunk in view.
            foreach (ChunkVector cv in activeChunks) DrawChunk(cv, gameTime, _spriteBatch, _camera);

            // Draw block selector.
            if (_gameWorld.GetSelection().HasValue) _selectionSprite.Draw(0, _spriteBatch, _gameWorld.GetSelection().Value, selectionColor);

            _spriteBatch.End();
        }

        private void DrawChunk(ChunkVector cv, GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            Chunk c = _gameWorld.GetChunk(cv);

            if (c != null) c.Draw(gameTime, spriteBatch, camera);
        }

        public void Dispose() {}
    }
}