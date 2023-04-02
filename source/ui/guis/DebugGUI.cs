using System.Diagnostics;
using Celesteia.Resources;
using Celesteia.UI;
using Celesteia.UI.Elements;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;

namespace Celesteia.GUIs {
    public class DebugGUI : GUI {
        private new GameInstance Game => (GameInstance) base.Game;
        public DebugGUI(GameInstance game) : base(game, Rect.ScreenFull) {}

        private Label updateLabel;
        private Label drawLabel;

        public override void LoadContent(ContentManager Content) {
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

            Root.AddChild(template.Clone().SetNewRect(template.GetRect().SetY(AbsoluteUnit.WithValue(textRow(0)))).SetText($"Celesteia {GameInstance.Version}"));
            Root.AddChild(updateLabel = template.Clone().SetNewRect(template.GetRect().SetY(AbsoluteUnit.WithValue(textRow(1)))).SetText(""));
            Root.AddChild(drawLabel = template.Clone().SetNewRect(template.GetRect().SetY(AbsoluteUnit.WithValue(textRow(2)))).SetText(""));
            
            Debug.WriteLine("Loaded Debug GUI.");
        }

        private double updateTime;
        private double lastUpdate;
        public override void Update(GameTime gameTime, out bool clickedAnything) {
            clickedAnything = false;

            updateTime = gameTime.TotalGameTime.TotalMilliseconds - lastUpdate;
            lastUpdate = gameTime.TotalGameTime.TotalMilliseconds;

            updateLabel.SetText($"Update: {updateTime.ToString("0.00")}ms");
        }
        
        private double drawTime;
        private double lastDraw;
        public override void Draw(GameTime gameTime)
        {
            drawTime = gameTime.TotalGameTime.TotalMilliseconds - lastDraw;
            lastDraw = gameTime.TotalGameTime.TotalMilliseconds;

            drawLabel.SetText($"Draw: {drawTime.ToString("0.00")}ms");

            if (GameInstance.DebugMode) base.Draw(gameTime);
        }
    }
}