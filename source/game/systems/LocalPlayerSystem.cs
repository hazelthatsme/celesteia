using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Components.Player;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class LocalPlayerSystem : EntityUpdateSystem
    {
        private ComponentMapper<TargetPosition> targetPositionMapper;
        private ComponentMapper<PhysicsEntity> physicsEntityMapper;
        private ComponentMapper<EntityAttributes> attributesMapper;
        private ComponentMapper<PlayerMovement> movementMapper;
        private ComponentMapper<LocalPlayer> localPlayerMapper;

        public LocalPlayerSystem() : base(Aspect.All(typeof(TargetPosition), typeof(PlayerMovement), typeof(PhysicsEntity), typeof(LocalPlayer))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            targetPositionMapper = mapperService.GetMapper<TargetPosition>();
            physicsEntityMapper = mapperService.GetMapper<PhysicsEntity>();
            attributesMapper = mapperService.GetMapper<EntityAttributes>();
            movementMapper = mapperService.GetMapper<PlayerMovement>();
            localPlayerMapper = mapperService.GetMapper<LocalPlayer>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                PlayerMovement input = movementMapper.Get(entityId);
                PhysicsEntity physicsEntity = physicsEntityMapper.Get(entityId);
                EntityAttributes.EntityAttributeMap attributes = attributesMapper.Get(entityId).Attributes;
                LocalPlayer localPlayer = localPlayerMapper.Get(entityId);

                if (physicsEntity.CollidingDown) localPlayer.JumpFuel = 1f;

                Vector2 movement = new Vector2(
                    input.TestHorizontal(),
                    0f
                );

                movement *= 1f + (input.TestRun() * 1.5f);
                movement *= attributes.Get(EntityAttribute.MovementSpeed);
                movement *= gameTime.GetElapsedSeconds();

                targetPositionMapper.Get(entityId).Target += movement;
                if (input.TestJump() > 0f && localPlayer.JumpFuel > 0f) {
                    localPlayer.JumpFuel -= gameTime.GetElapsedSeconds();
                    physicsEntity.SetVelocity(physicsEntity.Velocity.X, -10f);
                }
            }
        }
    }
}