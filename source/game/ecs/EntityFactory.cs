using Celesteia.Resources;
using Celesteia.Game.Systems.MainMenu;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Celesteia.Game.Skybox;
using Celesteia.Resources.Sprites;
using Celesteia.Game.Components.Player;
using MonoGame.Extended.TextureAtlases;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Game.Input;
using Celesteia.Game.Components;
using Microsoft.Xna.Framework.Input;
using Celesteia.Resources.Management;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Components.Items;
using Celesteia.Resources.Types;

namespace Celesteia.Game.ECS {
    /*
        Contains various commonly used prefabrications for entities.
        Many of the functions were moved to EntityTypes.
    */

    public class EntityFactory {
        private readonly World World;
        private readonly GameInstance Game;

        public EntityFactory(World world, GameInstance game) {
            World = world;
            Game = game;
        }

        public Entity CreateSkyboxPortion(string name, Color color, float rotation, float depth)
        {
            return new EntityBuilder(World)
                .AddComponent(new Transform2(Vector2.Zero, 0F, new Vector2(3f, 3f)))
                .AddComponent(new SkyboxRotateZ(rotation))
                .AddComponent(ResourceManager.Skybox.GetAsset(name).Frames.Clone().SetColor(color).SetDepth(depth))
                .Build();
        }

        public Entity CreateEntity(NamespacedKey key) => CreateEntity(ResourceManager.Entities.GetResource(key) as EntityType);

        public Entity CreateEntity(EntityType type)
        {
            Entity entity = World.CreateEntity();
            type.Instantiate(entity);
            
            return entity;
        }

        public static void BuildPlayer(Entity entity, Texture2D sprites) {
            entity.Attach(new Transform2());

            entity.Attach(new TargetPosition());

            entity.Attach(new EntityFrames(
                TextureAtlas.Create("player", sprites, 24, 24),
                0, 1,
                ResourceManager.SPRITE_SCALING
            ));

            entity.Attach(new EntityInventory(36, 
                new ItemStack(NamespacedKey.Base("iron_pickaxe"), 1),
                new ItemStack(NamespacedKey.Base("wooden_torch"), 10))
            );

            entity.Attach(new PhysicsEntity(1f, true));

            entity.Attach(new CollisionBox(1.5f, 3f));

            entity.Attach(new PlayerInput()
                .AddHorizontal(new KeyDefinition("Walk", Keys.A, Keys.D, KeyDetectType.Held))
                .AddRun(new KeyDefinition("Run", null, Keys.LeftShift, KeyDetectType.Held))
                .AddJump(new KeyDefinition("Jetpack", null, Keys.Space, KeyDetectType.Held))
                .AddInventory(new KeyDefinition("Inventory", null, Keys.B, KeyDetectType.Down))
                .AddCrafting(new KeyDefinition("Crafting", null, Keys.C, KeyDetectType.Down))
                .AddPause(new KeyDefinition("Pause", null, Keys.Escape, KeyDetectType.Down))
            );

            entity.Attach(new LocalPlayer());

            entity.Attach(new CameraFollow());

            entity.Attach(new EntityAttributes(new EntityAttributes.EntityAttributeMap()
                .Set(EntityAttribute.MovementSpeed, 5f)
                .Set(EntityAttribute.JumpFuel, .5f)
                .Set(EntityAttribute.JumpForce, 10f)
                .Set(EntityAttribute.BlockRange, 7f)
            ));
        }
    }
}