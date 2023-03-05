using Celesteia.Graphics;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {    
    public class EntityDebugSystem : EntityDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;

        private ComponentMapper<Transform2> transformMapper;

        private SpriteFont _font;

        public EntityDebugSystem(Camera2D camera, SpriteBatch spriteBatch) : base(Aspect.All(typeof(Transform2))) {
            _camera = camera;
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();

            _font = ResourceManager.Fonts.GetFontType("Hobo").Font;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            
            foreach (int entityId in ActiveEntities) {
                Transform2 transform = transformMapper.Get(entityId);

                _spriteBatch.DrawString(_font, transform.Position.ToString(), transform.Position, Color.White, 0f, new Vector2(0.5f, 0.5f), .12f, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();
        }
    }
}