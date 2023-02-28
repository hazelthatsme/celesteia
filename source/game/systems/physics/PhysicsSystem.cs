using Celesteia.Game.Components;
using Celesteia.Game.Worlds;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems.Physics {
    public class PhysicsSystem : EntityUpdateSystem {
        public const float GRAVITY_CONSTANT = 9.7f;

        public PhysicsSystem() : base(Aspect.All(typeof(PhysicsEntity), typeof(Transform2))) {}

        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<PhysicsEntity> physicsEntityMapper;

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            physicsEntityMapper = mapperService.GetMapper<PhysicsEntity>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                Transform2 transform = transformMapper.Get(entityId);
                PhysicsEntity physicsEntity = physicsEntityMapper.Get(entityId);

                transform.Position += physicsEntity.Velocity * gameTime.GetElapsedSeconds();
            }
        }
    }
}