using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Components.Player;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class LocalPlayerSystem : EntityUpdateSystem
    {
        private ComponentMapper<TargetPosition> targetPositionMapper;
        private ComponentMapper<PhysicsEntity> physicsEntityMapper;
        private ComponentMapper<EntityFrames> framesMapper;
        private ComponentMapper<EntityAttributes> attributesMapper;
        private ComponentMapper<PlayerMovement> movementMapper;
        private ComponentMapper<LocalPlayer> localPlayerMapper;

        public LocalPlayerSystem() : base(Aspect.All(typeof(TargetPosition), typeof(PhysicsEntity), typeof(EntityFrames), typeof(PlayerMovement), typeof(LocalPlayer))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            targetPositionMapper = mapperService.GetMapper<TargetPosition>();
            physicsEntityMapper = mapperService.GetMapper<PhysicsEntity>();
            framesMapper = mapperService.GetMapper<EntityFrames>();
            attributesMapper = mapperService.GetMapper<EntityAttributes>();
            movementMapper = mapperService.GetMapper<PlayerMovement>();
            localPlayerMapper = mapperService.GetMapper<LocalPlayer>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                PlayerMovement input = movementMapper.Get(entityId);
                PhysicsEntity physicsEntity = physicsEntityMapper.Get(entityId);
                EntityFrames frames = framesMapper.Get(entityId);
                EntityAttributes.EntityAttributeMap attributes = attributesMapper.Get(entityId).Attributes;
                LocalPlayer localPlayer = localPlayerMapper.Get(entityId);

                if (physicsEntity.CollidingDown) localPlayer.ResetJump();

                Vector2 movement = new Vector2(input.TestHorizontal(), 0f);

                if (movement.X != 0f) {
                    frames.Effects = movement.X < 0f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                }

                movement *= 1f + (input.TestRun() * 1.5f);
                movement *= attributes.Get(EntityAttribute.MovementSpeed);
                movement *= gameTime.GetElapsedSeconds();

                targetPositionMapper.Get(entityId).Target += movement;
                if (localPlayer.JumpRemaining > 0f) {
                    if (input.TestJump() > 0f) {
                        physicsEntity.SetVelocity(physicsEntity.Velocity.X, -attributes.Get(EntityAttribute.JumpForce));
                        localPlayer.JumpRemaining -= gameTime.GetElapsedSeconds();
                    }
                }
            }
        }
    }
}