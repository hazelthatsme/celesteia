using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Worlds {
    public class WorldManager : GameComponent
    {
        private new GameInstance Game => (GameInstance) base.Game;
        public WorldManager(GameInstance Game) : base(Game) {}

        private GameWorld _loaded;

        private void LoadWorld(GameWorld gameWorld) {
            if (_loaded != null) _loaded.Dispose();

            _loaded = gameWorld;
        }

        public void LoadNewWorld() {
            GameWorld gameWorld = new GameWorld(250, 75, Game);
            LoadWorld(gameWorld);
        }
    }
}