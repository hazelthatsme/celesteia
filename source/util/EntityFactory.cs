using Celestia.GameInput;
using Celestia.Screens.Components;
using Celestia.Screens.Systems;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace Celestia.Utilities.ECS {
    public class EntityFactory {
        private readonly World World;
        private readonly Game Game;

        public EntityFactory(World world, Game game) {
            World = world;
            Game = game;
        }

        public Entity CreatePlayer() {
            Texture2D playerSprites = Game.Content.Load<Texture2D>("sprites/entities/player/base_sample");
            TextureAtlas atlas = TextureAtlas.Create("playerAtlas", playerSprites, 16, 24);

            return new EntityBuilder(World)
                .AddComponent(new Transform2())
                .AddComponent(new Sprite(atlas.GetRegion(0)))
                .AddComponent(new InputTest(new KeyDefinition(Keys.A, Keys.D)))
                .AddComponent(new LocalPlayer())
                .AddComponent(new CameraFollow())
                .AddComponent(new EntityAttributes(new EntityAttributes.EntityAttributeMap()
                    .Set(EntityAttribute.MovementSpeed, 10f)
                ))
                .Build();
        }
    }
}