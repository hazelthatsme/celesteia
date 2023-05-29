using System;
using Celesteia.Game.Planets.Generation;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Planets {
    public class GeneratedPlanet : ChunkMap
    {
        public int Seed { get; private set; }

        private IChunkProvider _provider;

        public GeneratedPlanet(int w, int h, int? seed = null) : base(w, h)
        {
            Seed = seed.HasValue ? seed.Value : (int) System.DateTime.Now.Ticks;
        }

        public void Generate(IChunkProvider provider, Action<string> progressReport = null) {
            Action<string> doProgressReport = (s) => { if (progressReport != null) progressReport(s); };

            _provider = provider;
            doProgressReport("Generating chunks...");

            ChunkVector _cv;
            for (_cv.X = 0; _cv.X < Width; _cv.X++)
                for (_cv.Y = 0; _cv.Y < Height; _cv.Y++)
                    provider.ProvideChunk(Map[_cv.X, _cv.Y] = new Chunk(_cv));

            provider.GenerateStructures(doProgressReport);

            doProgressReport("Planet generated.");
        }
        
        public override Vector2 GetSpawnpoint() => _provider.GetSpawnpoint();
    }
}