using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class GameWorldRenderSystem : DrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private ChunkVector _pivotChunkPos => ChunkVector.FromVector2(_camera.Center);
        private int _renderDistance => 5;
        private GameWorld _gameWorld;

        public GameWorldRenderSystem(Camera2D camera, SpriteBatch spriteBatch, GameWorld world) {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _gameWorld = world;
        }

        private ChunkVector _v;
        public override void Draw(GameTime gameTime) {
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
                }
            }

            _spriteBatch.End();
        }

        private void DrawChunk(ChunkVector cv, GameTime gameTime, SpriteBatch spriteBatch, Camera2D camera) {
            Chunk c = _gameWorld.GetChunk(cv);

            if (c != null) c.Draw(gameTime, spriteBatch, camera);
        }
    }
}