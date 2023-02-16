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

        public Entity CreateChunk() {
            return new EntityBuilder(World)
                .AddComponent(new Chunk())
                .Build();
        }
    }
}