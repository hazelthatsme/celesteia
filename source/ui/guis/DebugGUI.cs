using Celesteia.Resources;
using Celesteia.UI;
using Celesteia.UI.Elements;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;

namespace Celesteia.GUIs {
    public class DebugGUI : GUI {
        private new GameInstance Game => (GameInstance) base.Game;
        public DebugGUI(GameInstance game) : base(game, Rect.ScreenFull) {}

        private double fps;
        private double lastUpdate;

        private Label fpsLabel;

        public override void LoadContent() {
            float fontSize = 12f;

            Label template = new Label(new Rect(
                AbsoluteUnit.WithValue(10),
                AbsoluteUnit.WithValue(10),
                AbsoluteUnit.WithValue(200),
                AbsoluteUnit.WithValue(50)
            ))
                .SetTextProperties(new TextProperties()
                    .SetColor(Color.White)
                    .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                    .SetFontSize(fontSize)
                    .SetTextAlignment(TextAlignment.Top | TextAlignment.Left)
                );
            float textSpacing = 4f;
            float textRow(int number) => 10f + number * (fontSize + textSpacing);

            Root.AddChild(template.Clone().SetNewRect(template.GetRect().SetY(AbsoluteUnit.WithValue(textRow(0)))).SetText("Celesteia"));
            Root.AddChild(fpsLabel = template.Clone().SetNewRect(template.GetRect().SetY(AbsoluteUnit.WithValue(textRow(1)))).SetText(""));
        }

        public override void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalSeconds - lastUpdate < 0.25) return;

            fps = 1 / (gameTime.ElapsedGameTime.TotalSeconds);

            fpsLabel.SetText("FPS: " + fps.ToString("0"));

            lastUpdate = gameTime.TotalGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            if (GameInstance.DebugMode) base.Draw(gameTime);
        }
    }
}