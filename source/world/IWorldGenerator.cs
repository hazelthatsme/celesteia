using Celesteia.Screens.Components.Game;

namespace Celesteia.World {
    public interface IWorldGenerator {
        // Get the game world associated with the generator.
        public GameWorld GetWorld();
        
        // Set the game world for reference.
        public void SetWorld(GameWorld world);

        // Get the naturally generated block at X and Y.
        public byte GetNaturalBlock(int x, int y);
    }
}