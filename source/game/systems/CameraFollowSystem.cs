using Celesteia.Game.Components;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class CameraFollowSystem : EntityUpdateSystem
    {
        private readonly Camera2D _camera;
        private Vector2 _target;

        private ComponentMapper<TargetPosition> targetPositionMapper;
        private ComponentMapper<CameraFollow> followMapper;

        public CameraFollowSystem(Camera2D camera) : base(Aspect.All(typeof(TargetPosition), typeof(CameraFollow))) {
            _camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            targetPositionMapper = mapperService.GetMapper<TargetPosition>();
            followMapper = mapperService.GetMapper<CameraFollow>();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 calculatedCenter = _camera.Center;
            float cumulativeWeight = 0f;

            foreach (int entityId in ActiveEntities) {
                float weight = followMapper.Get(entityId).weight;
                calculatedCenter = targetPositionMapper.Get(entityId).Target * weight;
                cumulativeWeight += weight;
            }

            calculatedCenter /= cumulativeWeight;

            _target = calculatedCenter;
            _camera.MoveTo(_target);
        }
    }
}