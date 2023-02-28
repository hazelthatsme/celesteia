using Celesteia.Graphics;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class CameraRenderSystem : EntityDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;

        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<EntityFrames> entityFramesMapper;

        public CameraRenderSystem(Camera2D camera, SpriteBatch spriteBatch) : base(Aspect.All(typeof(Transform2), typeof(EntityFrames))) {
            _camera = camera;
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            entityFramesMapper = mapperService.GetMapper<EntityFrames>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            
            foreach (int entityId in ActiveEntities) {
                Transform2 transform = transformMapper.Get(entityId);
                EntityFrames entityFrames = entityFramesMapper.Get(entityId);

                entityFrames.Draw(0, _spriteBatch, transform.Position, Color.White);
            }

            _spriteBatch.End();
        }
    }
}