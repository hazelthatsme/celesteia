using System;
using System.Threading.Tasks;
using Celesteia.Game.ECS;
using Celesteia.Game.Planets;
using Celesteia.Game.Planets.Generation;
using Microsoft.Xna.Framework;

namespace Celesteia.Game {
    public class WorldManager : GameComponent
    {
        private new GameInstance Game => (GameInstance) base.Game;
        public WorldManager(GameInstance Game) : base(Game) {}

        private GameWorld _loaded;

        private GameWorld LoadWorld(GameWorld gameWorld) {
            if (_loaded != null) _loaded.Dispose();

            return _loaded = gameWorld;
        }

        public async Task<GameWorld> LoadNewWorld(Action<string> progressReport = null, int? seed = null) {
            // Asynchronously generate the world.
            GameWorld generatedWorld = await Task.Run<GameWorld>(() => {
                GeneratedPlanet planet = new GeneratedPlanet(250, 75, seed);
                planet.Generate(new TerranPlanetGenerator(planet), progressReport);
                return new GameWorld(planet);
            });

            return LoadWorld(generatedWorld);
        }
    }
}