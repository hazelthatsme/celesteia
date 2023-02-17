using System;
using Celestia.Resources;
using Celestia.Resources.Sprites;
using Celestia.Screens.Systems;
using Celestia.Screens.Systems.MainMenu;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Celestia.Utilities.ECS {
    public class EntityFactory {
        private readonly World World;
        private readonly Game Game;

        public EntityFactory(World world, Game game) {
            World = world;
            Game = game;
        }

        public Entity CreateChunk() {
            return new EntityBuilder(World)
                .AddComponent(new Chunk())
                .Build();
        }

        public Entity CreateSkyboxPortion(string name, Color color, float rotation, float depth)
        {
            return new EntityBuilder(World)
                .AddComponent(new Transform2())
                .AddComponent(new MainMenuRotateZ(rotation))
                .AddComponent(ResourceManager.Skybox.GetAsset(name).Frames.Clone().SetColor(color).SetDepth(depth))
                .Build();
        }
    }
}