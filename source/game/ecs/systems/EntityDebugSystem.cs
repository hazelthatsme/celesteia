using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace Celesteia.Game.ECS.Systems {    
    public class EntityDebugSystem : EntityDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;

        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<Sprite> spriteMapper;

        public EntityDebugSystem(SpriteFont font, Camera2D camera, SpriteBatch spriteBatch) : base(Aspect.All(typeof(Transform2))) {
            _font = font;
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

                _spriteBatch.DrawString(_font, transform.Position.ToString(), transform.Position, Color.White, 0f, new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None, 0f);
            }

            _spriteBatch.DrawString(_font, _camera.Center.ToString(), _camera.Center, Color.White, 0f, new Vector2(0.5f, 0.5f), 0.12f, SpriteEffects.None, 0f);

            _spriteBatch.End();
        }
    }
}