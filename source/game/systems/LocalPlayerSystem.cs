using System;
using System.Collections.Generic;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Components.Player;
using Celesteia.Game.Input;
using Celesteia.Game.Items;
using Celesteia.Game.World;
using Celesteia.Graphics;
using Celesteia.GUIs.Game;
using Celesteia.Resources;
using Celesteia.Resources.Sprites;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class LocalPlayerSystem : UpdateSystem, IDrawSystem
    {   
        private GameInstance _game;
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
                inventory = _player.Get<Inventory>();
            }
        }
        private LocalPlayer localPlayer;
        private PlayerInput input;
        private PhysicsEntity physicsEntity;
        private EntityFrames frames;
        private EntityAttributes attributes;
        private TargetPosition targetPosition;
        private Inventory inventory;
        
        private SpriteBatch _spriteBatch;

        private BlockFrame _selectionSprite;

        public LocalPlayerSystem(GameInstance game, GameWorld world, Camera2D camera, SpriteBatch spriteBatch, GameGUI gameGui) {
            _game = game;
            _world = world;
            _camera = camera;
            _gameGui = gameGui;
            _spriteBatch = spriteBatch;

            _selectionSprite = ResourceManager.Blocks.Selection.GetFrame(0);
        }

        private bool IsGameActive => !_gameGui.Paused && (int)_gameGui.State < 1 && _game.IsActive;

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
            
            UpdateMouse(gameTime, input, clicked);
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
            
            if (!KeyboardHelper.IsDown(Keys.LeftControl) && MouseHelper.ScrollDelta != 0f) {
                int change = (int) -Math.Clamp(MouseHelper.ScrollDelta, -1f, 1f);
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
            _inventoryPress = input.Inventory.Poll();
            _craftingPress = input.Crafting.Poll();
            _pausePress = input.Pause.Poll();

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
            h = input.Horizontal.Poll();

            if (h != 0f) {
                // Player has started moving, animate.
                if (!_moving) _startedMoving = gameTime.TotalGameTime.TotalSeconds;

                // Flip sprite according to horizontal movement float.
                frames.Effects = h < 0f ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }

            _moving = h != 0f;

            // If the player is moving, change the frame every .25 seconds, else return the standing frame.
            frames.Frame = _moving ? (int)((gameTime.TotalGameTime.TotalSeconds - _startedMoving) / 0.25) : 0;

            if (h == 0f) return;

            h *= 1f + (input.Run.Poll() ? 1.5f : 0);
            h *= attributes.Get(EntityAttribute.MovementSpeed);
            h *= gameTime.GetElapsedSeconds();

            targetPosition.Target.X += h;
        }

        private void UpdateJump(GameTime gameTime, LocalPlayer localPlayer, PlayerInput input, PhysicsEntity physicsEntity, EntityAttributes.EntityAttributeMap attributes)
        {
            if (localPlayer.JumpRemaining > 0f) {
                if (input.Jump.Poll()) {
                    physicsEntity.SetVelocity(physicsEntity.Velocity.X, -attributes.Get(EntityAttribute.JumpForce));
                    localPlayer.JumpRemaining -= gameTime.GetElapsedSeconds();
                }
            } else if (physicsEntity.CollidingDown) localPlayer.JumpRemaining = attributes.Get(EntityAttribute.JumpFuel);
        }

        private Point? Selection;
        private BlockType SelectedBlock;
        private Color SelectionColor;
        public void SetSelection(Point? selection) {
            if (selection == Selection) return;
            
            Selection = selection;
            if (!selection.HasValue) {
                SelectionColor = Color.Transparent;
                return;
            }

            SelectedBlock = ResourceManager.Blocks.GetBlock(_world.ChunkMap.GetForeground(Selection.Value));
            if (SelectedBlock.Frames == null) SelectedBlock = ResourceManager.Blocks.GetBlock(_world.ChunkMap.GetBackground(Selection.Value));

            SelectionColor = (SelectedBlock.Strength >= 0 ? Color.White : Color.Black);
        }

        Vector2 pointV = Vector2.Zero;
        Point point = Point.Zero;
        private void UpdateMouse(GameTime gameTime, PlayerInput input, bool clicked) {
            pointV = _camera.ScreenToWorld(MouseHelper.Position);
            pointV.Floor();
            point = pointV.ToPoint();
            
            SetSelection(IsGameActive ? point : null);

            if (!clicked && IsGameActive) UpdateClick(gameTime, input);
        }


        bool success = false;
        ItemStack stack = null;
        IItemActions actions = null;
        private void UpdateClick(GameTime gameTime, PlayerInput input) {
            if (!input.PrimaryUse.Poll() && !input.SecondaryUse.Poll()) return;
            if (_gameGui.GetSelectedItem() == null) return;

            stack = _gameGui.GetSelectedItem();
            if(stack.Type == null || stack.Type.Actions == null) return;

            actions = stack.Type.Actions;

            if (input.PrimaryUse.Poll()) success = actions.Primary(gameTime, _world, point, _player);
            if (input.SecondaryUse.Poll()) success = stack.Type.Actions.Secondary(gameTime, _world, point, _player);

            if (success && stack.Type.ConsumeOnUse) stack.Amount -= 1;

            inventory.AssertAmounts();
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());

            _selectionSprite.Draw(0, _spriteBatch, pointV, SelectionColor);

            _spriteBatch.End();
        }
    }
}