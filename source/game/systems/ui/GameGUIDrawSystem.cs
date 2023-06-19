using Celesteia.GUIs.Game;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems.UI {
    public class GameGUIDrawSystem : DrawSystem {
        private GameGUI _gui;

        public GameGUIDrawSystem(GameGUI gui) => _gui = gui;
        public override void Draw(GameTime gameTime) => _gui.Draw(gameTime);
    }
}