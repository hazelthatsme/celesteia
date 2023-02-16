using System.Diagnostics;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.GUIs {
    public class DebugGUI : GUI {
        private new Game Game => (Game) base.Game;
        public DebugGUI(Game game) : base(game) {}

        public static Vector2 camCenter;

        private SpriteFont debugFont;

        private double fps;
        private double lastUpdate;

        private Label fpsLabel;
        private Label cameraCenterLabel;

        public override void LoadContent() {
            debugFont = Game.Content.Load<SpriteFont>("Hobo");

            fpsLabel = new Label(
                new Rect(AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(200), AbsoluteUnit.WithValue(50)),
                "",
                TextAlignment.Top | TextAlignment.Left,
                debugFont
            );
            cameraCenterLabel = new Label(
                new Rect(AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(32), AbsoluteUnit.WithValue(200), AbsoluteUnit.WithValue(50)),
                "",
                TextAlignment.Top | TextAlignment.Left,
                debugFont
            );

            elements.Add(fpsLabel);
            elements.Add(cameraCenterLabel);
        }

        public override void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalSeconds - lastUpdate < 0.25) return;

            fps = 1 / (gameTime.ElapsedGameTime.TotalSeconds);

            fpsLabel.text = "FPS: " + fps.ToString("0");
            cameraCenterLabel.text = "Camera Center: " + camCenter.ToString();

            lastUpdate = gameTime.TotalGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Game.DebugMode) base.Draw(gameTime);
        }
    }
}