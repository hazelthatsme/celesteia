using Celesteia.Resources;
using Celesteia.Screens;
using Celesteia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.GUIs {
    public class MainMenu : GUI {
        private new Game Game => (Game) base.Game;
        public MainMenu(Game game) : base(game, Rect.ScreenFull) {}

        private Texture2D logo;
        private Texture2D buttonTexture;

        private IContainer CenteredMenu;
        private IContainer LogoPivot;

        public override void LoadContent() {
            logo = Game.Content.Load<Texture2D>("celesteia/logo");
            buttonTexture = Game.Content.Load<Texture2D>("sprites/ui/button");

            float logoRatio = logo.Height / (float) logo.Width;

            CenteredMenu = new Container(new Rect(
                new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                AbsoluteUnit.WithValue(250f),
                AbsoluteUnit.WithValue(0f)
            ));

            Root.AddChild(CenteredMenu);


            CenteredMenu.AddChild(LogoPivot = new Container(new Rect(
                AbsoluteUnit.WithValue(0f),
                new ScreenSpaceUnit(-.25f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(0f)
            )));

            LogoPivot.AddChild(
                new Image(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(0f),
                    new ScreenSpaceUnit(.4f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                    new ScreenSpaceUnit(.4f * logoRatio, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal)
                ))
                .SetTexture(logo)
                .SetColor(Color.White)
                .SetPivotPoint(new Vector2(.5f))
            );

            float buttonHeight = 56f;
            float buttonSpacing = 20f;
            float buttonRow(int number) => (number) * buttonHeight + buttonSpacing;

            CenteredMenu.AddChild(
                new Button(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(0)),
                    new RelativeUnit(1f, CenteredMenu.GetRect(), RelativeUnit.Orientation.Horizontal),
                    AbsoluteUnit.WithValue(56f)
                ))
                .SetPivotPoint(new Vector2(.5f))
                .SetOnClick((position) => { Game.LoadScreen(new GameplayScreen(Game)); })
                .SetTexture(buttonTexture)
                .MakePatches(4)
                .SetText("Start Game")
                .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                .SetFontSize(24f)
                .SetTextAlignment(TextAlignment.Center)
                .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Violet, Color.DarkViolet))
            );

            CenteredMenu.AddChild(
                new Button(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(1)),
                    new RelativeUnit(1f, CenteredMenu.GetRect(), RelativeUnit.Orientation.Horizontal),
                    AbsoluteUnit.WithValue(56f)
                ))
                .SetPivotPoint(new Vector2(.5f))
                //.SetOnClick((position) => { Game.Exit(); })
                .SetTexture(buttonTexture)
                .MakePatches(4)
                .SetText("Options")
                .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                .SetFontSize(24f)
                .SetTextAlignment(TextAlignment.Center)
                .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Violet, Color.DarkViolet))
            );

            CenteredMenu.AddChild(
                new Button(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(2)),
                    new RelativeUnit(1f, CenteredMenu.GetRect(), RelativeUnit.Orientation.Horizontal),
                    AbsoluteUnit.WithValue(56f)
                ))
                .SetPivotPoint(new Vector2(.5f))
                .SetOnClick((position) => { Game.Exit(); })
                .SetTexture(buttonTexture)
                .MakePatches(4)
                .SetText("Quit Game")
                .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                .SetFontSize(24f)
                .SetTextAlignment(TextAlignment.Center)
                .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Red, Color.DarkRed))
            );

            base.LoadContent();
        }
    }
}