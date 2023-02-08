using System;
using System.Diagnostics;
using Celestia.GameInput;
using Celestia.GUIs;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

namespace Celestia.Screens {
    public class SplashScreen : GameScreen {
        private new Game Game => (Game) base.Game;

        Texture2D leafalLogo;
        Image backgroundImage;
        Image logoElement;
        Rect logoRect;

        private float logoRatio;

        public SplashScreen(Game game) : base(game) {}

        public override void LoadContent()
        {
            base.LoadContent();

            leafalLogo = Game.Content.Load<Texture2D>("branding/leafal/TextLogo");

            logoRatio = leafalLogo.Height / (float) leafalLogo.Width;

            backgroundImage = new Image(new Rect(
                new ScreenSpaceUnit(0f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(0f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                new ScreenSpaceUnit(1f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical)
            ), null, Color.Black, 0f);

            logoRect = new Rect(
                new ScreenSpaceUnit(0.25f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(0.5f - (logoRatio / 2f), ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                new ScreenSpaceUnit(0.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(logoRatio * 0.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal)
            );
            logoElement = new Image(logoRect, leafalLogo, Color.White, 1f);
        }
        
        private float timeElapsed = 0f;
        private float fadeInTime = 1.5f;
        private float fadeOutTime = 1.5f;
        private float duration = 5f;
        private float endTimeout = 1f;
        private float progress = 0f;
        private Color color = Color.White;

        public override void Update(GameTime gameTime) {
            if (progress >= 1f || Input.GetAny()) {
                Game.LoadScreen(new MainMenuScreen(Game));
                return;
            }

            timeElapsed += (float) (gameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
            float alpha = 1f;
            if (timeElapsed <= fadeInTime) alpha = Math.Min(timeElapsed / fadeInTime, 1f);
            if (duration - fadeOutTime <= timeElapsed) alpha = Math.Max((duration - timeElapsed) / fadeOutTime, 0f);

            color.A = (byte) ((int) (alpha * 255));

            progress = timeElapsed / (duration + endTimeout);
            UpdateLogoRect(progress);
        }

        private float growFactor = 0f;
        private void UpdateLogoRect(float progress) {
            Rect r = logoElement.GetRect();

            r.X.SetValue(0.25f - (progress * (growFactor / 2f)));
            r.Y.SetValue(0.5f - (logoRatio / 2f) - ((logoRatio / 2f) * progress * (growFactor / 2f)));
            r.Width.SetValue(0.5f + (progress * growFactor));
            r.Height.SetValue(0.5f * logoRatio + ((progress * growFactor * logoRatio)));

            logoElement.SetRect(r);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp);

            backgroundImage.Draw(Game.SpriteBatch);
            
            logoElement.color = color;
            logoElement.Draw(Game.SpriteBatch);

            Game.SpriteBatch.End();
        }

        public override void Dispose()
        {
            Debug.WriteLine("Unloading SplashScreen content...");
            base.UnloadContent();
            Debug.WriteLine("Disposing SplashScreen...");
            base.Dispose();
        }
    }
}