using Celesteia.Resources;
using Celesteia.Screens;
using Celesteia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.GUIs {
    public class MainMenu : GUI {
        private new Game Game => (Game) base.Game;
        public MainMenu(Game game) : base(game) {}

        private Texture2D logo;
        private Texture2D buttonTexture;

        public override void LoadContent() {
            logo = Game.Content.Load<Texture2D>("celesteia/logo");
            buttonTexture = Game.Content.Load<Texture2D>("sprites/ui/button");

            float logoRatio = logo.Height / (float) logo.Width;

            Elements.Add(new Image(new Rect(
                new ScreenSpaceUnit(.3f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.4f * logoRatio, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal)
            ), logo, Color.White, 1f, ImageFitMode.Contain));

            Elements.Add(
                new Button(new Rect(
                    new ScreenSpaceUnit(.3f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                    new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                    new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                    new ScreenSpaceUnit(.1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical)
                ))
                .SetOnClick((position) => { Game.LoadScreen(new GameplayScreen(Game)); })
                .SetTexture(buttonTexture)
                .MakePatches(4)
                .SetText("Start Game")
                .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                .SetFontSize(24f)
                .SetTextAlignment(TextAlignment.Center)
                .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Blue, Color.Violet))
            );

            Elements.Add(
                new Button(new Rect(
                    new ScreenSpaceUnit(.3f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                    new ScreenSpaceUnit(.55f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                    new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                    new ScreenSpaceUnit(.1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical)
                ))
                .SetOnClick((position) => { Game.Exit(); })
                .SetTexture(buttonTexture)
                .MakePatches(4)
                .SetText("Quit Game")
                .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                .SetFontSize(24f)
                .SetTextAlignment(TextAlignment.Center)
                .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Red, Color.Violet))
            );

            base.LoadContent();
        }
    }
}