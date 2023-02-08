using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace Celestia.Screens.Systems {
    public class CameraRenderSystem : EntityDrawSystem
    {
        private readonly OrthographicCamera _camera;
        private readonly SpriteBatch _spriteBatch;

        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Sprite> spriteMapper;

        public CameraRenderSystem(OrthographicCamera camera, SpriteBatch spriteBatch) : base(Aspect.All(typeof(Transform2), typeof(Sprite))) {
            _camera = camera;
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            spriteMapper = mapperService.GetMapper<Sprite>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            
            foreach (int entityId in ActiveEntities) {
                Transform2 transform = transformMapper.Get(entityId);
                Sprite sprite = spriteMapper.Get(entityId);

                sprite.Draw(_spriteBatch, transform.Position, transform.Rotation, transform.Scale);
            }

            _spriteBatch.End();
        }
    }
}