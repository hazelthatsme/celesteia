using Celesteia.Game.Components.Physics;
using Celesteia.Game.Worlds;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems.Physics {
    public class PhysicsGravitySystem : EntityUpdateSystem {
        private GameWorld _gameWorld;

        public PhysicsGravitySystem(GameWorld gameWorld) : base(Aspect.All(typeof(PhysicsEntity), typeof(Transform2))) {
            _gameWorld = gameWorld;
        }

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
                PhysicsEntity physicsEntity = physicsEntityMapper.Get(entityId);

                if (physicsEntity.CollidingDown) {
                    physicsEntity.SetVelocity(physicsEntity.Velocity.X, 0f);
                }
                if (!physicsEntity.Gravity) continue;

                Vector2 gravity = new Vector2(0f, physicsEntity.Mass * PhysicsSystem.GRAVITY_CONSTANT);
                gravity *= gameTime.GetElapsedSeconds();

                physicsEntity.AddVelocity(gravity);
            }
        }
    }
}