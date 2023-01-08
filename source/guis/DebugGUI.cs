using System.Diagnostics;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.GUIs {
    public class DebugGUI : GUI {
        private SpriteFont arial;

        private double fps;
        private double lastUpdate;

        private Label fpsLabel;

        public override void Load(ContentManager contentManager) {
            arial = contentManager.Load<SpriteFont>("Arial");

            fpsLabel = new Label(
                new Rect(AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(10), AbsoluteUnit.WithValue(200), AbsoluteUnit.WithValue(50)),
                "",
                TextAlignment.Top | TextAlignment.Left,
                arial
            );

            elements.Add(fpsLabel);
        }

        public override void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalSeconds - lastUpdate < 0.25) return;

            fps = 1 / (gameTime.ElapsedGameTime.TotalSeconds);

            fpsLabel.text = "FPS: " + fps.ToString("0");

            lastUpdate = gameTime.TotalGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Game.DebugMode) base.Draw(spriteBatch);
        }
    }
}