using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Celesteia.Resources.Sprites;
using Celesteia.Game.Components.Player;
using MonoGame.Extended.TextureAtlases;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Game.Input;
using Celesteia.Game.Components;
using Microsoft.Xna.Framework.Input;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Components.Skybox;
using Celesteia.Resources.Types;
using Celesteia.Game.Input.Definitions.Keyboard;
using Celesteia.Game.Input.Definitions.Gamepad;
using Celesteia.Game.Input.Definitions.Mouse;
using MonoGame.Extended.Input;
using Celesteia.Game.Input.Definitions;
using Celesteia.Game.Input.Conditions;

namespace Celesteia.Game.ECS {
    /*
        Contains various commonly used prefabrications for entities.
        Many of the functions were moved to EntityTypes.
    */

    public class EntityFactory {
        private readonly GameWorld _gameWorld;

        public EntityFactory(GameWorld gameWorld) => _gameWorld = gameWorld;

        public Entity CreateEntity(NamespacedKey key) => CreateEntity(ResourceManager.Entities.GetResource(key) as EntityType);

        public Entity CreateEntity(EntityType type)
        {
            Entity entity = _gameWorld.CreateEntity();
            type.Instantiate(entity);
            
            return entity;
        }

        public static void BuildPlayer(Entity entity, Texture2D sprites) {
            entity.Attach(new Transform2());

            entity.Attach(new TargetPosition());

            entity.Attach(new EntityFrames(
                TextureAtlas.Create("player", sprites, 24, 24),
                0, 2,
                ResourceManager.SPRITE_SCALING
            ));

            entity.Attach(new Inventory(36, 
                new ItemStack(NamespacedKey.Base("iron_pickaxe"), 1),
                new ItemStack(NamespacedKey.Base("wooden_torch"), 10)
            ));

            entity.Attach(new PhysicsEntity(1.6f, true));

            entity.Attach(new CollisionBox(1.5f, 3f));

            entity.Attach(new PlayerInput() {
                Horizontal = new AverageCondition(
                    new TrinaryKeyboardDefinition() { Negative = Keys.A, Positive = Keys.D, PollType = InputPollType.Held },
                    new SensorGamepadDefinition() { Sensor = GamePadSensor.LeftThumbStickX }
                ),
                Run = new AnyCondition(
                    new BinaryKeyboardDefinition() { Keys = Keys.LeftShift, PollType = InputPollType.Held },
                    new BinaryGamepadDefinition() { Buttons = Buttons.LeftShoulder, PollType = InputPollType.Held }
                ),
                Jump = new AnyCondition(
                    new BinaryKeyboardDefinition() { Keys = Keys.Space, PollType = InputPollType.Held },
                    new BinaryGamepadDefinition() { Buttons = Buttons.A, PollType = InputPollType.Held }
                ),
                Inventory = new AnyCondition(
                    new BinaryKeyboardDefinition() { Keys = Keys.B, PollType = InputPollType.Pressed },
                    new BinaryGamepadDefinition() { Buttons = Buttons.Y, PollType = InputPollType.Pressed }
                ),
                Crafting = new AnyCondition(
                    new BinaryKeyboardDefinition() { Keys = Keys.C, PollType = InputPollType.Pressed },
                    new BinaryGamepadDefinition() { Buttons = Buttons.X, PollType = InputPollType.Pressed }
                ),
                Pause = new AnyCondition(
                    new BinaryKeyboardDefinition() { Keys = Keys.Escape, PollType = InputPollType.Pressed },
                    new BinaryGamepadDefinition() { Buttons = Buttons.Start, PollType = InputPollType.Pressed }
                ),
                PrimaryUse = new AnyCondition(
                    new BinaryMouseDefinition() { Button = MouseButton.Left, PollType = InputPollType.Held },
                    new BinaryGamepadDefinition() { Buttons = Buttons.RightTrigger, PollType = InputPollType.Held }
                ),
                SecondaryUse = new AnyCondition(
                    new BinaryMouseDefinition() { Button = MouseButton.Right, PollType = InputPollType.Held },
                    new BinaryGamepadDefinition() { Buttons = Buttons.LeftTrigger, PollType = InputPollType.Held }
                )
            });

            entity.Attach(new LocalPlayer());

            entity.Attach(new CameraFollow());

            entity.Attach(new EntityAttributes(new EntityAttributes.EntityAttributeMap()
                .Set(EntityAttribute.MovementSpeed, 12.5f)
                .Set(EntityAttribute.JumpFuel, .5f)
                .Set(EntityAttribute.JumpForce, 10f)
                .Set(EntityAttribute.BlockRange, 7f)
            ));
        }
    }
}