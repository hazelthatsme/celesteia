using Celestia.Screens;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.GUIs {
    public class MainMenu : GUI {
        private Texture2D logo;
        private SpriteFont arialBold;
        private Game gameRef;

        public MainMenu(Game gameRef) {
            this.gameRef = gameRef;
        }

        public override void Load(ContentManager contentManager) {
            logo = contentManager.Load<Texture2D>("celestia/logo");
            arialBold = contentManager.Load<SpriteFont>("ArialBold");

            float logoRatio = logo.Height / (float) logo.Width;

            elements.Add(new Image(new Rect(
                new ScreenSpaceUnit(.3f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.4f * logoRatio, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal)
            ), logo, Color.White, 1f, ImageFitMode.Contain));

            elements.Add(new Button(new Rect(
                new ScreenSpaceUnit(.3f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical)
            ),
            (position) => { gameRef.LoadScreen(new GameScreen(gameRef)); }, null, "Start Game", TextAlignment.Center, arialBold));

            elements.Add(new Button(new Rect(
                new ScreenSpaceUnit(.3f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.55f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical)
            ),
            (position) => { gameRef.Exit(); }, null, "Quit Game", TextAlignment.Center, arialBold));
        }
    }
}