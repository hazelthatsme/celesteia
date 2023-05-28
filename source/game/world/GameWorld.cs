using System;
using Celesteia.Game.Components.Items;
using Celesteia.Game.World.Planet;
using Celesteia.Game.World.Planet.Generation;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Celesteia.Game.World {
    public class GameWorld : IDisposable {

        public ChunkMap ChunkMap { get; private set; }
        private IChunkProvider _provider;

        public int Seed { get; private set; }

        public GameWorld(int width, int height, int? seed = null) {
            Seed = seed.HasValue ? seed.Value : (int) System.DateTime.Now.Ticks;
            ChunkMap = new ChunkMap(width, height);
        }

        public void Generate(IChunkProvider provider, Action<string> progressReport = null) {
            Action<string> doProgressReport = (s) => { if (progressReport != null) progressReport(s); };

            _provider = provider;
            doProgressReport("Generating chunks...");

            ChunkVector _cv;
            for (_cv.X = 0; _cv.X < ChunkMap.Width; _cv.X++)
                for (_cv.Y = 0; _cv.Y < ChunkMap.Height; _cv.Y++)
                    provider.ProvideChunk(ChunkMap.Map[_cv.X, _cv.Y] = new Chunk(_cv));

            provider.GenerateStructures(doProgressReport);
        }

        public RectangleF? TestBoundingBox(int x, int y, byte id) {
            RectangleF? box = ResourceManager.Blocks.GetBlock(id).BoundingBox;

            if (!box.HasValue) return null;

            return new RectangleF(
                x, y,
                box.Value.Width, box.Value.Height
            );
        }
        public RectangleF? TestBoundingBox(int x, int y) => TestBoundingBox(x, y, ChunkMap.GetForeground(x, y));


        public Vector2 GetSpawnpoint() => _provider.GetSpawnpoint();

        public void Dispose() {
            ChunkMap = null;
        }
    }
}