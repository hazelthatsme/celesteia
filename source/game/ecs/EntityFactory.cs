using Celesteia.Resources;
using Celesteia.Game.Systems.MainMenu;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Celesteia.Game.Skybox;

namespace Celesteia.Game.ECS {
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
    }
}