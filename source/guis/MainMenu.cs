using Celestia.Screens;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.GUIs {
    public class MainMenu : GUI {
        private new Game Game => (Game) base.Game;
        public MainMenu(Game game) : base(game) {}

        private Texture2D logo;
        private SpriteFont arialBold;

        public override void LoadContent() {
            logo = Game.Content.Load<Texture2D>("celestia/logo");
            arialBold = Game.Content.Load<SpriteFont>("ArialBold");

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
            (position) => { Game.LoadScreen(new GameplayScreen(Game)); }, null, "Start Game", TextAlignment.Center, arialBold));

            elements.Add(new Button(new Rect(
                new ScreenSpaceUnit(.3f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.55f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical)
            ),
            (position) => { Game.Exit(); }, null, "Quit Game", TextAlignment.Center, arialBold));

            base.LoadContent();
        }
    }
}