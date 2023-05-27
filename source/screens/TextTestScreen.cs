using System;
using System.Diagnostics;
using Celesteia.Game.Input;
using Celesteia.Resources;
using Celesteia.UI;
using Celesteia.UI.Elements;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace Celesteia.Screens {
    public class TextTestScreen : GameScreen {
        private new GameInstance Game => (GameInstance) base.Game;
        public TextTestScreen(GameInstance game) : base(game) {}

        private IContainer Root;

        private Label topLeft;
        //private Label topCenter;
        //private Label topRight;
        //private Label middleLeft;
        //private Label middle;
        //private Label middleRight;
        //private Label bottomLeft;
        //private Label bottom;
        private Label bottomRight;

        private TextProperties properties;
        private float _fontSize = 24f;

        public override void LoadContent()
        {
            base.LoadContent();

            Root = new Container(Rect.ScreenFull);

            properties = new TextProperties().SetColor(Color.White).SetFont(ResourceManager.Fonts.GetFontType("Hobo")).SetText("Hello, world!");

            topLeft = new Label(new Rect(AbsoluteUnit.WithValue(50), AbsoluteUnit.WithValue(50), AbsoluteUnit.WithValue(100), AbsoluteUnit.WithValue(100)));
            Root.AddChild(topLeft);
            bottomRight = new Label(new Rect(AbsoluteUnit.WithValue(50), AbsoluteUnit.WithValue(150), AbsoluteUnit.WithValue(100), AbsoluteUnit.WithValue(100)));
            Root.AddChild(bottomRight);
        }

        public override void Update(GameTime gameTime) {
            _fontSize += MouseHelper.ScrollDelta;
            _fontSize = Math.Clamp(_fontSize, 1f, 100f);

            topLeft.SetTextProperties(properties.Clone().SetTextAlignment(TextAlignment.Top | TextAlignment.Left).SetFontSize(_fontSize));
            bottomRight.SetTextProperties(properties.Clone().SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right).SetFontSize(_fontSize));
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            
            Root.Draw(Game.SpriteBatch);

            Game.SpriteBatch.End();
        }

        public override void Dispose()
        {
            Debug.WriteLine("Unloading TextTestScreen content...");
            base.UnloadContent();
            Debug.WriteLine("Disposing TextTestScreen...");
            base.Dispose();
        }
    }
}