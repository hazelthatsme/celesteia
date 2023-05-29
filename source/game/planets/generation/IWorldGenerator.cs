using System;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Planets.Generation {
    public interface IChunkProvider {
        // Provide a chunk's tile map.
        public void ProvideChunk(Chunk c);

        // Get the natural block and wall at X and Y.
        public byte[] GetNaturalBlocks(int x, int y);

        // Get a safe spot to spawn the player.
        public Vector2 GetSpawnpoint();

        // Generate various structures in the world.
        public void GenerateStructures(Action<string> progressReport = null);
    }
}