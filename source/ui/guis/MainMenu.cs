using Celesteia.Resources;
using Celesteia.Screens;
using Celesteia.UI;
using Celesteia.UI.Elements;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.GUIs {
    public class MainMenu : GUI {
        public MainMenu(GameInstance game) : base(game, Rect.ScreenFull) {}

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
            float buttonSpacing = 10f;
            float buttonRow(int number) => number * (buttonHeight + buttonSpacing);

            Button template = new Button(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(0)),
                    new RelativeUnit(1f, CenteredMenu.GetRect(), RelativeUnit.Orientation.Horizontal),
                    AbsoluteUnit.WithValue(56f)
                ))
                .SetPivotPoint(new Vector2(.5f))
                .SetTexture(buttonTexture)
                .MakePatches(4)
                .SetTextProperties(new TextProperties()
                    .SetColor(Color.White)
                    .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                    .SetFontSize(24f)
                    .SetTextAlignment(TextAlignment.Center))
                .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Violet, Color.DarkViolet));

            CenteredMenu.AddChild(
                template.Clone()
                    .SetOnMouseUp((button, position) => { Game.LoadScreen(
                        new GameplayScreen(Game), 
                        new MonoGame.Extended.Screens.Transitions.FadeTransition(Game.GraphicsDevice, Color.Black)
                    ); })
                    .SetText("Start Game")
            );

            CenteredMenu.AddChild(
                template.Clone()
                    .SetNewRect(template.GetRect().SetY(AbsoluteUnit.WithValue(buttonRow(1))))
                    .SetText("Options")
            );

            CenteredMenu.AddChild(
                template.Clone()
                    .SetNewRect(template.GetRect().SetY(AbsoluteUnit.WithValue(buttonRow(2))))
                    .SetOnMouseUp((button, position) => { Game.Exit(); })
                    .SetText("Quit Game")
                    .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Red, Color.DarkRed))
            );

            base.LoadContent();
        }
    }
}