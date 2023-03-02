using Celesteia.GUIs.Game;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems.UI {
    public class GameGUISystem : IUpdateSystem, IDrawSystem
    {
        private GameGUI _gui;

        public GameGUISystem(GameGUI gui) {
            _gui = gui;
        }

        public void Initialize(World world) { }

        public void Draw(GameTime gameTime)
        {
            _gui.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _gui.Update(gameTime);
        }

        public void Dispose()
        {
            _gui = null;
        }
    }
}