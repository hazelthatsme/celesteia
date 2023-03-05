using Celesteia.Game.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class TargetPositionSystem : EntityUpdateSystem {
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<TargetPosition> targetPositionMapper;

        public TargetPositionSystem() : base(Aspect.All(typeof(Transform2), typeof(TargetPosition))) {}

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            targetPositionMapper = mapperService.GetMapper<TargetPosition>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                TargetPosition targetPosition = targetPositionMapper.Get(entityId);
                Transform2 transform = transformMapper.Get(entityId);

                transform.Position = targetPosition.Target;
            }
        }
    }
}