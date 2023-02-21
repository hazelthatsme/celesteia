using Celesteia.Game;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Screens.Systems {
    public class GameWorldSystem : IUpdateSystem, IDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private ChunkVector _pivotChunkPos => ChunkVector.FromVector2(_camera.Center);
        private int _renderDistance => 15 - _camera.Zoom;
        private GameWorld _gameWorld;

        public GameWorldSystem(Camera2D camera, SpriteBatch spriteBatch, GameWorld world) {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _gameWorld = world;
        }

        public void Initialize(World world) {}

        private ChunkVector _prevChunkPos = new ChunkVector(int.MinValue, int.MinValue);
        public void Update(GameTime gameTime) {
            if (_prevChunkPos != _pivotChunkPos) {
                _gameWorld.UpdateChunks(_pivotChunkPos, _renderDistance);

                _prevChunkPos = _pivotChunkPos;
            }

            _gameWorld.DisableNextChunk();
        }

        public void Draw(GameTime gameTime) {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());

            _gameWorld.Draw(gameTime, _spriteBatch, _camera);

            _spriteBatch.End();
        }

        public void Dispose() {
            _gameWorld.Dispose();
        }
    }
}