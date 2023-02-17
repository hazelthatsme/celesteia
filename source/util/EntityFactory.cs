using Celestia.Screens.Systems;
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
    }
}