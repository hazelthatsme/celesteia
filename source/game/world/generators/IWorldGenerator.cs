namespace Celesteia.Game.World.Generators {
    public interface IWorldGenerator {
        // Get the world.
        public GameWorld GetWorld();

        // Set the world.
        public void SetWorld(GameWorld world);

        // Get the natural block at X and Y.
        public byte GetNaturalBlock(int x, int y);
    }
}