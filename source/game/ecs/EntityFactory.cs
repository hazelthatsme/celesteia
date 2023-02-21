using Celesteia.Resources;
using Celesteia.Screens.Systems.MainMenu;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.ECS {
    public class EntityFactory {
        private readonly MonoGame.Extended.Entities.World World;
        private readonly GameInstance Game;

        public EntityFactory(MonoGame.Extended.Entities.World world, GameInstance game) {
            World = world;
            Game = game;
        }

        public Entity CreateSkyboxPortion(string name, Color color, float rotation, float depth)
        {
            return new EntityBuilder(World)
                .AddComponent(new Transform2(Vector2.Zero, 0F, new Vector2(3f, 3f)))
                .AddComponent(new MainMenuRotateZ(rotation))
                .AddComponent(ResourceManager.Skybox.GetAsset(name).Frames.Clone().SetColor(color).SetDepth(depth))
                .Build();
        }
    }
}