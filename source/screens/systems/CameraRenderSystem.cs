using System;
using Celesteia.Graphics;
using Celesteia.Resources;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Screens.Systems {
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

                entityFrames.Draw(0, _spriteBatch, GetDrawingPosition(transform.Position), Color.White);
            }

            _spriteBatch.End();
        }

        public Vector2 GetDrawingPosition(Vector2 position) {
            return new Vector2(
                (float)Math.Round(position.X * ResourceManager.INVERSE_SPRITE_SCALING) / ResourceManager.INVERSE_SPRITE_SCALING,
                (float)Math.Round(position.Y * ResourceManager.INVERSE_SPRITE_SCALING) / ResourceManager.INVERSE_SPRITE_SCALING
            );
        }
    }
}