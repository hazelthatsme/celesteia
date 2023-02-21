using System;
using Celesteia.Graphics;
using Celesteia.Resources;
using Celesteia.Screens.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Screens.Systems {
    public class CameraFollowSystem : EntityUpdateSystem
    {
        private readonly Camera2D _camera;
        private Vector2 _current;
        private Vector2 _target;

        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<CameraFollow> followMapper;

        public CameraFollowSystem(Camera2D camera) : base(Aspect.All(typeof(Transform2), typeof(CameraFollow))) {
            _camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            followMapper = mapperService.GetMapper<CameraFollow>();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 calculatedCenter = _camera.Center;
            float cumulativeWeight = 0f;

            foreach (int entityId in ActiveEntities) {
                float weight = followMapper.Get(entityId).weight;
                calculatedCenter = transformMapper.Get(entityId).Position * weight;
                cumulativeWeight += weight;
            }

            calculatedCenter /= cumulativeWeight;

            _target = calculatedCenter;

            // Move camera smoothly.
            //_current = Vector2.Lerp(_current, _target, gameTime.GetElapsedSeconds() * Camera2D.FOLLOW_SPEED);
            _camera.MoveTo(GetDrawingPosition(_target));
        }

        public Vector2 GetDrawingPosition(Vector2 position) {
            return new Vector2(
                (float)Math.Round(position.X * ResourceManager.INVERSE_SPRITE_SCALING) / ResourceManager.INVERSE_SPRITE_SCALING,
                (float)Math.Round(position.Y * ResourceManager.INVERSE_SPRITE_SCALING) / ResourceManager.INVERSE_SPRITE_SCALING
            );
        }
    }
}