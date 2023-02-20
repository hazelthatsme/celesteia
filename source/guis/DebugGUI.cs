using System.Diagnostics;
using Celesteia.Resources;
using Celesteia.Resources.Types;
using Celesteia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.GUIs {
    public class DebugGUI : GUI {
        private new Game Game => (Game) base.Game;
        public DebugGUI(Game game) : base(game, Rect.ScreenFull) {}

        private FontType debugFont;

        private double fps;
        private double lastUpdate;

        private Label fpsLabel;

        public override void LoadContent() {
            debugFont = ResourceManager.Fonts.GetFontType("Hobo");

            Root.AddChild(new Label(
                new Rect(AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(200), AbsoluteUnit.WithValue(50)),
                "Celesteia",
                TextAlignment.Top | TextAlignment.Left,
                debugFont
            ));

            Root.AddChild(fpsLabel = new Label(
                new Rect(AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(27), AbsoluteUnit.WithValue(200), AbsoluteUnit.WithValue(50)),
                "",
                TextAlignment.Top | TextAlignment.Left,
                debugFont
            ));
        }

        public override void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalSeconds - lastUpdate < 0.25) return;

            fps = 1 / (gameTime.ElapsedGameTime.TotalSeconds);

            fpsLabel.text = "FPS: " + fps.ToString("0");

            lastUpdate = gameTime.TotalGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game.DebugMode) base.Draw(gameTime);
        }
    }
}