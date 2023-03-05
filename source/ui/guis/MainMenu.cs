using System.Diagnostics;
using Celesteia.Game.Worlds;
using Celesteia.Resources;
using Celesteia.Screens;
using Celesteia.UI;
using Celesteia.UI.Elements;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.GUIs {
    public class MainMenu : GUI {
        public MainMenu(GameInstance game) : base(game, Rect.ScreenFull) {}

        private Texture2D logo;
        private Texture2D buttonTexture;

        private IContainer MainScreen;
        private IContainer OptionsScreen;
        private IContainer NewWorldScreen;

        private IContainer LogoPivot;
        private Label Progress;

        private Button buttonTemplate;
        private float buttonRow(int number) => number * (buttonHeight + buttonSpacing);
        private float buttonHeight = 56f;
        private float buttonSpacing = 10f;

        public override void LoadContent(ContentManager Content) {
            logo = Game.Content.Load<Texture2D>("celesteia/logo");
            buttonTexture = Game.Content.Load<Texture2D>("sprites/ui/button");

            buttonTemplate = new Button(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(0)),
                    AbsoluteUnit.WithValue(250f),
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

            // Load all the screens.
            LoadMainScreen();
            LoadOptionsScreen();
            LoadNewWorldScreen();

            base.LoadContent(Content);
        }
        
        private void LoadMainScreen() {
            Root.AddChild(MainScreen = new Container(Rect.ScreenFull));
            MainScreen.SetEnabled(true);

            IContainer menu = new Container(new Rect(
                new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                AbsoluteUnit.WithValue(250f),
                AbsoluteUnit.WithValue(0f)
            ));

            menu.AddChild(LogoPivot = new Container(new Rect(
                AbsoluteUnit.WithValue(0f),
                new ScreenSpaceUnit(-.25f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(0f)
            )));

            float logoRatio = logo.Height / (float) logo.Width;
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

            menu.AddChild(
                buttonTemplate.Clone()
                    .SetNewRect(buttonTemplate.GetRect()
                        .SetY(AbsoluteUnit.WithValue(buttonRow(0)))
                        .SetWidth(new RelativeUnit(1f, menu.GetRect(), RelativeUnit.Orientation.Horizontal))
                    )
                    .SetOnMouseUp(async (button, position) => {
                        ShowNewWorldScreen();
                        Debug.WriteLine("Generating world...");
                        GameWorld _gameWorld = await Game.Worlds.LoadNewWorld((progressReport) => {
                            Progress.SetText(progressReport);
                            Debug.WriteLine("  " + progressReport);
                        });
                        Game.LoadScreen(
                            new GameplayScreen(Game, _gameWorld), 
                            new MonoGame.Extended.Screens.Transitions.FadeTransition(Game.GraphicsDevice, Color.Black)
                        ); 
                    })
                    .SetText("Start Game")
            );

            menu.AddChild(
                buttonTemplate.Clone()
                    .SetNewRect(buttonTemplate.GetRect()
                        .SetY(AbsoluteUnit.WithValue(buttonRow(1)))
                        .SetWidth(new RelativeUnit(1f, menu.GetRect(), RelativeUnit.Orientation.Horizontal))
                    )
                    .SetOnMouseUp((button, position) => { ShowOptionsScreen(); })
                    .SetText("Options")
            );

            menu.AddChild(
                buttonTemplate.Clone()
                    .SetNewRect(buttonTemplate.GetRect()
                        .SetY(AbsoluteUnit.WithValue(buttonRow(2)))
                        .SetWidth(new RelativeUnit(1f, menu.GetRect(), RelativeUnit.Orientation.Horizontal))
                    )
                    .SetOnMouseUp((button, position) => { Game.Exit(); })
                    .SetText("Quit Game")
                    .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Red, Color.DarkRed))
            );

            MainScreen.AddChild(menu);
        }
        private void ShowMainScreen() {
            MainScreen.SetEnabled(true);
            OptionsScreen.SetEnabled(false);
            NewWorldScreen.SetEnabled(false);
        }

        private void LoadOptionsScreen() {
            Root.AddChild(OptionsScreen = new Container(Rect.ScreenFull));
            OptionsScreen.SetEnabled(false);

            IContainer menu = new Container(new Rect(
                new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                AbsoluteUnit.WithValue(450f),
                AbsoluteUnit.WithValue(0f)
            ));

            menu.AddChild(
                buttonTemplate.Clone()
                    .SetNewRect(buttonTemplate.GetRect()
                        .SetY(AbsoluteUnit.WithValue(buttonRow(0)))
                        .SetWidth(new RelativeUnit(1f, menu.GetRect(), RelativeUnit.Orientation.Horizontal))
                    )
                    .SetOnMouseUp((button, position) => { ShowMainScreen(); })
                    .SetText("Back to Main Menu")
            );

            OptionsScreen.AddChild(menu);
        }
        private void ShowOptionsScreen() {
            MainScreen.SetEnabled(false);
            OptionsScreen.SetEnabled(true);
            NewWorldScreen.SetEnabled(false);
        }

        private void LoadNewWorldScreen() {
            Root.AddChild(NewWorldScreen = new Container(Rect.ScreenFull));
            NewWorldScreen.SetEnabled(false);

            NewWorldScreen.AddChild(Progress = new Label(
                new Rect(
                    new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                    new ScreenSpaceUnit(.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                    AbsoluteUnit.WithValue(200),
                    AbsoluteUnit.WithValue(50)
                ))
                .SetPivotPoint(new Vector2(0.5f, 0.5f))
                .SetTextProperties(new TextProperties()
                    .SetColor(Color.White)
                    .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                    .SetFontSize(24f)
                    .SetTextAlignment(TextAlignment.Center)
                )
                .SetText("")
            );
        }
        private void ShowNewWorldScreen() {
            MainScreen.SetEnabled(false);
            OptionsScreen.SetEnabled(false);
            NewWorldScreen.SetEnabled(true);
        }
    }
}