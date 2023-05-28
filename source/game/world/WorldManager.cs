using System;
using System.Threading.Tasks;
using Celesteia.Game.World.Planet.Generation;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.World {
    public class WorldManager : GameComponent
    {
        private new GameInstance Game => (GameInstance) base.Game;
        public WorldManager(GameInstance Game) : base(Game) {}

        private GameWorld _loaded;

        private GameWorld LoadWorld(GameWorld gameWorld) {
            if (_loaded != null) _loaded.Dispose();

            return _loaded = gameWorld;
        }

        public async Task<GameWorld> LoadNewWorld(Action<string> progressReport = null) {
            // Asynchronously generate the world.
            GameWorld generatedWorld = await Task.Run<GameWorld>(() => {
                GameWorld gameWorld = new GameWorld(250, 75);
                gameWorld.Generate(new TerranWorldGenerator(gameWorld.ChunkMap, gameWorld.Seed), progressReport);
                
                if (progressReport != null) progressReport.Invoke("World generated.");

                return gameWorld;
            });

            return LoadWorld(generatedWorld);
        }
    }
}