using Celesteia.Game.Components.Items;
using Celesteia.GUIs.Game;
using Celesteia.Resources;
using Celesteia.Screens;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;

namespace Celesteia.UI.Elements.Game {
    public class PauseMenu : Container {
        private Image background;
        private IContainer centerMenu;
        private GameGUI _gameGui;

        private float buttonRow(int number) => number * (buttonHeight + buttonSpacing);
        private float buttonHeight = 56f;
        private float buttonSpacing = 10f;

        public PauseMenu(GameGUI gameGui, Rect rect, Button buttonTemplate) : base(rect) {
            _gameGui = gameGui;

            background = new Image(Rect.RelativeFull(rect)).SetColor(new Color(0, 0, 0, 100));
            AddChild(background);

            centerMenu = new Container(new Rect(
                new RelativeUnit(0.5f, GetRect(), RelativeUnit.Orientation.Horizontal),
                new RelativeUnit(0.5f, GetRect(), RelativeUnit.Orientation.Vertical),
                AbsoluteUnit.WithValue(350f),
                AbsoluteUnit.WithValue(2 * (buttonHeight + buttonSpacing) - buttonSpacing)
            ));
            AddChild(centerMenu);

            AddButtons(buttonTemplate);
        }

        private void AddButtons(Button template) {
            centerMenu.AddChild(new Label(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(-1)),
                    new RelativeUnit(1f, centerMenu.GetRect(), RelativeUnit.Orientation.Horizontal),
                    AbsoluteUnit.WithValue(buttonHeight)
                ))
                .SetPivotPoint(new Vector2(0.5f, 0.5f))
                .SetTextProperties(new TextProperties().SetColor(Color.White).SetFont(ResourceManager.Fonts.GetFontType("Hobo")).SetFontSize(24f).SetTextAlignment(TextAlignment.Center))
                .SetText("Paused")
            );

            centerMenu.AddChild(template.Clone()
                .SetNewRect(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(0)),
                    new RelativeUnit(1f, centerMenu.GetRect(), RelativeUnit.Orientation.Horizontal),
                    AbsoluteUnit.WithValue(buttonHeight)
                ))
                .SetText("Back to Game")
                .SetOnMouseUp((button, point) => {
                    _gameGui.TogglePause();
                })
            );

            centerMenu.AddChild(template.Clone()
                .SetNewRect(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(buttonRow(1)),
                    new RelativeUnit(1f, centerMenu.GetRect(), RelativeUnit.Orientation.Horizontal),
                    AbsoluteUnit.WithValue(buttonHeight)
                ))
                .SetText("Return to Title")
                .SetOnMouseUp((button, point) => {
                    _gameGui.Game.LoadScreen(new MainMenuScreen(_gameGui.Game),new MonoGame.Extended.Screens.Transitions.FadeTransition(_gameGui.Game.GraphicsDevice, Color.Black));
                })
            );
        }
    }
}