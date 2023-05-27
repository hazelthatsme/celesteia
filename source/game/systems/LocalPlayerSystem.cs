using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Xna.Framework.Input;
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
        public Entity Player {
            get => _player;
            set {
                _player = value;

                localPlayer = _player.Get<LocalPlayer>();
                targetPosition = _player.Get<TargetPosition>();
                physicsEntity = _player.Get<PhysicsEntity>();
                frames = _player.Get<EntityFrames>();
                attributes = _player.Get<EntityAttributes>();
                input = _player.Get<PlayerInput>();
                inventory = _player.Get<EntityInventory>();
            }
        }
        private LocalPlayer localPlayer;
        private PlayerInput input;
        private PhysicsEntity physicsEntity;
        private EntityFrames frames;
        private EntityAttributes attributes;
        private TargetPosition targetPosition;
        private EntityInventory inventory;

        private InputManager _input;

        public LocalPlayerSystem(GameWorld world, Camera2D camera, GameGUI gameGui, InputManager input) {
            _world = world;
            _camera = camera;
            _gameGui = gameGui;

            _input = input;
        }

        private bool IsGameActive => !_gameGui.Paused && (int)_gameGui.State < 1;

        public override void Update(GameTime gameTime)
        {
            if (_player == null) return;

            bool clicked = false;
            UpdateGUI(gameTime, input, out clicked);
            
            if (IsGameActive) {
                UpdateSelectedItem();
                
                UpdateMovement(gameTime, input, physicsEntity, frames, attributes.Attributes, targetPosition);
                UpdateJump(gameTime, localPlayer, input, physicsEntity, attributes.Attributes);
            }
            
            UpdateMouse(gameTime, clicked);
        }

        private static Dictionary<int, int> hotbarMappings = new Dictionary<int, int>() {
            { (int)Keys.D1, 0 },
            { (int)Keys.D2, 1 },
            { (int)Keys.D3, 2 },
            { (int)Keys.D4, 3 },
            { (int)Keys.D5, 4 },
            { (int)Keys.D6, 5 },
            { (int)Keys.D7, 6 },
            { (int)Keys.D8, 7 },
            { (int)Keys.D9, 8 }
        };

        private void UpdateSelectedItem() {
            foreach (int keys in hotbarMappings.Keys) {
                if (KeyboardHelper.Pressed((Keys) keys)) {
                    _gameGui.HotbarSelection = hotbarMappings[keys];
                }
            }
            
            if (!KeyboardHelper.IsDown(Keys.LeftControl) && _input.Mouse.ScrollDelta != 0f) {
                int change = _input.Mouse.ScrollDelta > 0f ? -1 : 1;
                int selection = _gameGui.HotbarSelection;

                selection += change;

                if (selection < 0) selection = _gameGui.HotbarSlots - 1;
                if (selection >= _gameGui.HotbarSlots) selection = 0;

                _gameGui.HotbarSelection = selection;
            }
        }

        bool _inventoryPress;
        bool _craftingPress;
        bool _pausePress;
        private void UpdateGUI(GameTime gameTime, PlayerInput input, out bool clicked) {
            _inventoryPress = input.TestInventory() > 0f;
            _craftingPress = input.TestCrafting() > 0f;
            _pausePress = input.TestPause() > 0f;

            if (_inventoryPress || _craftingPress || _pausePress) {
                switch (_gameGui.State) {
                    case InventoryScreenState.Closed:
                        if (_craftingPress) _gameGui.State = InventoryScreenState.Crafting;
                        else if (_inventoryPress) _gameGui.State = InventoryScreenState.Inventory;
                        else if (_pausePress) _gameGui.TogglePause();
                        break;
                    case InventoryScreenState.Inventory:
                        if (_craftingPress) _gameGui.State = InventoryScreenState.Crafting;
                        else _gameGui.State = InventoryScreenState.Closed;
                        break;
                    case InventoryScreenState.Crafting:
                        _gameGui.State = InventoryScreenState.Closed;
                        break;
                    default: break;
                }
            }

            _gameGui.Update(gameTime, out clicked);
        }

        float h;
        private bool _moving;
        private double _startedMoving;
        private void UpdateMovement(GameTime gameTime, PlayerInput input, PhysicsEntity physicsEntity, EntityFrames frames, EntityAttributes.EntityAttributeMap attributes, TargetPosition targetPosition) {
            h = input.TestHorizontal();
            Vector2 movement = new Vector2(h, 0f);

            if (h != 0f && !_moving) {
                // Player has started moving, animate.
                _startedMoving = gameTime.TotalGameTime.TotalSeconds;
                frames.Effects = movement.X < 0f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }

            _moving = movement != Vector2.Zero;

            // If the player is moving, change the frame every .25 seconds, else return the standing frame.
            frames.Frame = _moving ? (int)((gameTime.TotalGameTime.TotalSeconds - _startedMoving) / 0.25) : 0;

            if (h == 0f) return;

            movement *= 1f + (input.TestRun() * 1.5f);
            movement *= attributes.Get(EntityAttribute.MovementSpeed);
            movement *= gameTime.GetElapsedSeconds();

            targetPosition.Target += movement;
        }

        private void UpdateJump(GameTime gameTime, LocalPlayer localPlayer, PlayerInput input, PhysicsEntity physicsEntity, EntityAttributes.EntityAttributeMap attributes)
        {
            if (localPlayer.JumpRemaining > 0f) {
                if (input.TestJump() > 0f) {
                    physicsEntity.SetVelocity(physicsEntity.Velocity.X, -attributes.Get(EntityAttribute.JumpForce));
                    localPlayer.JumpRemaining -= gameTime.GetElapsedSeconds();
                }
            } else if (physicsEntity.CollidingDown) localPlayer.JumpRemaining = attributes.Get(EntityAttribute.JumpFuel);
        }

        Vector2 point = Vector2.Zero;
        private void UpdateMouse(GameTime gameTime, bool clicked) {
            point = _camera.ScreenToWorld(_input.Mouse.Position);
            
            if (IsGameActive) _world.SetSelection(point);
            else _world.SetSelection(null);

            if (!clicked && IsGameActive) UpdateClick(gameTime);
        }


        bool mouseClick = false;
        ItemStack stack = null;
        private void UpdateClick(GameTime gameTime) {
            mouseClick = _input.Mouse.GetMouseHeld(MouseButton.Left) || _input.Mouse.GetMouseHeld(MouseButton.Right);

            if (!mouseClick) return;

            stack = _gameGui.GetSelectedItem();

            if (stack == null || stack.Type == null || stack.Type.Actions == null) return;

            if (mouseClick) {
                bool success = false;
                        
                if (_input.Mouse.GetMouseHeld(MouseButton.Left)) success = stack.Type.Actions.OnLeftClick(gameTime, _world, point, _player);
                else if (_input.Mouse.GetMouseHeld(MouseButton.Right)) success = stack.Type.Actions.OnRightClick(gameTime, _world, point, _player);

                if (success && stack.Type.ConsumeOnUse) stack.Amount -= 1;

                inventory.Inventory.AssertAmounts();
            }
        }
    }
}