using System.Diagnostics;
using Celestia.GameInput;
using Celestia.UI;
using Celestia.GUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Screens {
    public class MainMenuScreen : IScreen
    {
        private Game gameRef;
        private MainMenu mainMenu;

        public MainMenuScreen(Game gameRef) {
            this.gameRef = gameRef;

            this.mainMenu = new MainMenu(gameRef);
        }

        public void Load(ContentManager contentManager)
        {
            this.mainMenu.Load(contentManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.mainMenu.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            this.mainMenu.Update(gameTime);
            
        }

        public void Dispose() {
            Debug.WriteLine("Disposing MainMenuScreen...");
        }

        public SamplerState GetSamplerState()
        {
            return SamplerState.PointClamp;
        }
    }
}