using System;
using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Components.Player;
using Celesteia.Game.Input;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Celesteia.GUIs.Game;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Systems {
    public class LocalPlayerSystem : UpdateSystem
    {
        private GameGUI _gameGui;
        private Camera2D _camera;
        private GameWorld _world;
        private Entity _player;

        private LocalPlayer localPlayer;
        private PlayerInput input;
        private PhysicsEntity physicsEntity;
        private EntityFrames frames;
        private EntityAttributes attributes;
        private TargetPosition targetPosition;
        private EntityInventory inventory;

        public LocalPlayerSystem(GameGUI gameGui, Camera2D camera, GameWorld world) {
            _gameGui = gameGui;
            _camera = camera;
            _world = world;
        }

        public void SetLocalPlayer(Entity player) {
            _player = player;

            localPlayer = _player.Get<LocalPlayer>();
            targetPosition = _player.Get<TargetPosition>();
            physicsEntity = _player.Get<PhysicsEntity>();
            frames = _player.Get<EntityFrames>();
            attributes = _player.Get<EntityAttributes>();
            input = _player.Get<PlayerInput>();
            inventory = _player.Get<EntityInventory>();
        }

        public override void Update(GameTime gameTime)
        {
            if (_player == null) return;

            bool clicked = false;
            UpdateGUI(gameTime, input, out clicked);

            UpdateMovement(gameTime, input, physicsEntity, frames, attributes.Attributes, targetPosition);
            UpdateJump(gameTime, localPlayer, input, physicsEntity, attributes.Attributes);

            if (!clicked) {
                Vector2 point = _camera.ScreenToWorld(MouseWrapper.GetPosition());
                ItemStack stack = _gameGui.GetSelectedItem();

                if (stack == null || stack.Type == null || stack.Type.Actions == null) return;

                bool mouseClick = MouseWrapper.GetMouseHeld(MouseButton.Left) || MouseWrapper.GetMouseHeld(MouseButton.Right);

                if (mouseClick) {
                    bool success = false;
                    
                    if (MouseWrapper.GetMouseHeld(MouseButton.Left)) success = stack.Type.Actions.OnLeftClick(gameTime, _world, point, _player);
                    else if (MouseWrapper.GetMouseHeld(MouseButton.Right)) success = stack.Type.Actions.OnRightClick(gameTime, _world, point, _player);

                    if (success && stack.Type.ConsumeOnUse) stack.Amount -= 1;

                    inventory.Inventory.AssertAmounts();
                }
            }
        }

        private void UpdateGUI(GameTime gameTime, PlayerInput input, out bool clicked) {
            

            _gameGui.Update(gameTime, out clicked);
        }

        private void UpdateMovement(GameTime gameTime, PlayerInput input, PhysicsEntity physicsEntity, EntityFrames frames, EntityAttributes.EntityAttributeMap attributes, TargetPosition targetPosition) {
            Vector2 movement = new Vector2(input.TestHorizontal(), 0f);

            if (movement.X != 0f) {
                frames.Effects = movement.X < 0f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }

            movement *= 1f + (input.TestRun() * 1.5f);
            movement *= attributes.Get(EntityAttribute.MovementSpeed);
            movement *= gameTime.GetElapsedSeconds();

            targetPosition.Target += movement;
        }

        private void UpdateJump(GameTime gameTime, LocalPlayer localPlayer, PlayerInput input, PhysicsEntity physicsEntity, EntityAttributes.EntityAttributeMap attributes)
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