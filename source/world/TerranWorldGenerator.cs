using Celesteia.Screens.Components.Game;

namespace Celesteia.World {
    public class TerranWorldGenerator : IWorldGenerator
    {
        private GameWorld _world;
        public GameWorld GetWorld() => _world;
        public void SetWorld(GameWorld world) => _world = world;

        public TerranWorldGenerator(GameWorld world) {
            SetWorld(world);
        }

        public byte GetNaturalBlock(int x, int y)
        {
            return (byte) ((x == 0 || y == 0 || x == _world.GetWidth() - 1 || y == _world.GetHeight() - 1) ? 3 : 0);
        }
    }
}