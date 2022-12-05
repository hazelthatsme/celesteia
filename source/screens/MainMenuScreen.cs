using System.Diagnostics;
using Celestia.UI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Screens {
    public class MainMenuScreen : IScreen
    {
        private Game gameRef;
        private Menu mainMenu;
        private SpriteFont arialBold;

        public MainMenuScreen(Game gameRef) {
            this.gameRef = gameRef;

            this.mainMenu = new Menu();
        }

        public void Load(ContentManager contentManager)
        {
            arialBold = contentManager.Load<SpriteFont>("ArialBold");
            this.mainMenu.elements.Add(new Button(new Rect(10f, 10f, 200f, 50f), (position) => { gameRef.Exit(); }, null, "Quit Game", TextAlignment.Center, arialBold));
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.mainMenu.Draw(spriteBatch);
            return;
        }

        public void Update(float deltaTime)
        {
            if (!Input.MouseButtons.Equals(MouseButtonState.None)) {
                this.mainMenu.ResolveMouseClick(Input.MousePosition, Input.MouseButtons);
            }
            return;
        }

        public void Dispose() {
            Debug.WriteLine("Disposing MainMenuScreen...");
        }
    }
}