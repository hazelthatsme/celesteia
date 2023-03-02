using System;
using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Components.Player;
using Celesteia.GUIs.Game;
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

        private GameGUI _gameGui;

        public LocalPlayerSystem(GameGUI gameGui) : base(Aspect.All(typeof(TargetPosition), typeof(PhysicsEntity), typeof(EntityFrames), typeof(PlayerMovement), typeof(LocalPlayer))) {
            _gameGui = gameGui;
        }

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
            bool clicked = false;

            _gameGui.Update(gameTime, out clicked);

            foreach (int entityId in ActiveEntities) {
                LocalPlayer localPlayer = localPlayerMapper.Get(entityId);
                PlayerMovement input = movementMapper.Get(entityId);
                PhysicsEntity physicsEntity = physicsEntityMapper.Get(entityId);
                EntityFrames frames = framesMapper.Get(entityId);
                EntityAttributes.EntityAttributeMap attributes = attributesMapper.Get(entityId).Attributes;
                TargetPosition targetPosition = targetPositionMapper.Get(entityId);

                UpdateMovement(gameTime, input, physicsEntity, frames, attributes, targetPosition);
                UpdateJump(gameTime, localPlayer, input, physicsEntity, attributes);
            }
        }

        private void UpdateMovement(GameTime gameTime, PlayerMovement input, PhysicsEntity physicsEntity, EntityFrames frames, EntityAttributes.EntityAttributeMap attributes, TargetPosition targetPosition) {
            Vector2 movement = new Vector2(input.TestHorizontal(), 0f);

            if (movement.X != 0f) {
                frames.Effects = movement.X < 0f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }

            movement *= 1f + (input.TestRun() * 1.5f);
            movement *= attributes.Get(EntityAttribute.MovementSpeed);
            movement *= gameTime.GetElapsedSeconds();

            targetPosition.Target += movement;
        }

        private void UpdateJump(GameTime gameTime, LocalPlayer localPlayer, PlayerMovement input, PhysicsEntity physicsEntity, EntityAttributes.EntityAttributeMap attributes)
        {
            if (physicsEntity.CollidingDown) localPlayer.ResetJump();

            if (localPlayer.JumpRemaining > 0f) {
                if (input.TestJump() > 0f) {
                    physicsEntity.SetVelocity(physicsEntity.Velocity.X, -attributes.Get(EntityAttribute.JumpForce));
                    localPlayer.JumpRemaining -= gameTime.GetElapsedSeconds();
                }
            }
        }
    }
}