using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Screens.Systems {
    public class WorldDrawingSystem : EntityDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;

        private ComponentMapper<Chunk> chunkMapper;

        public WorldDrawingSystem(Camera2D camera, SpriteBatch spriteBatch) : base(Aspect.All(typeof(Chunk))) {
            _camera = camera;
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            chunkMapper = mapperService.GetMapper<Chunk>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            
            foreach (int entityId in ActiveEntities) {
                Chunk chunk = chunkMapper.Get(entityId);

                chunk.Draw(gameTime, _spriteBatch, _camera);
            }

            _spriteBatch.End();
        }
    }
}