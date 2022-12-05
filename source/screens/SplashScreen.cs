using System;
using System.Diagnostics;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Screens {
    public class SplashScreen : IScreen {
        private Game gameRef;

        SpriteFont arialBold;
        Texture2D leafalLogo;
        Image backgroundImage;
        Image logoElement;
        Rect logoRect;

        public SplashScreen(Game gameRef) {
            this.gameRef = gameRef;
        }

        public void Load(ContentManager contentManager)
        {
            arialBold = contentManager.Load<SpriteFont>("ArialBold");
            leafalLogo = contentManager.Load<Texture2D>("branding/leafal/TextLogo");

            float heightToWidthRatio = leafalLogo.Height / (float) leafalLogo.Width;
            float verticalOffset = 0.5f - (heightToWidthRatio / 2f);

            backgroundImage = new Image(new ScreenSpaceRect(0f, 0f, 1f, 1f), null, Color.Black, 0f);

            logoRect = UpdateLogoRect();
            logoElement = new Image(logoRect, leafalLogo, Color.White, 1f);
        }
        
        private float timeElapsed = 0f;
        private float fadeInTime = 1.5f;
        private float fadeOutTime = 1.5f;
        private float duration = 5f;
        private float endTimeout = 1f;
        private float progress = 0f;
        private Color color = Color.White;

        public void Update(float deltaTime) {
            logoElement.SetRect(UpdateLogoRect());

            timeElapsed += deltaTime;
            float alpha = 1f;
            if (timeElapsed <= fadeInTime) alpha = Math.Min(timeElapsed / fadeInTime, 1f);
            if (duration - fadeOutTime <= timeElapsed) alpha = Math.Max((duration - timeElapsed) / fadeOutTime, 0f);

            color.A = (byte) ((int) (alpha * 255));

            progress = timeElapsed / (duration + endTimeout);
            if (progress >= 1f) {
                gameRef.LoadScreen(new MainMenuScreen(gameRef));
            }
        }

        private Rect UpdateLogoRect() {
            float heightToWidthRatio = (float) leafalLogo.Height / (float) leafalLogo.Width;
            float width = 0.5f * Game.GetGameWindow().ClientBounds.Width;
            float height = heightToWidthRatio * width;

            return new Rect(
                0.25f * Game.GetGameWindow().ClientBounds.Width,
                (0.5f - (heightToWidthRatio / 2f)) * Game.GetGameWindow().ClientBounds.Height,
                width,
                height
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            backgroundImage.Draw(spriteBatch);
            
            logoElement.color = color;
            logoElement.Draw(spriteBatch);
        }

        public void Dispose()
        {
            Debug.WriteLine("Disposing SplashScreen...");
        }
    }
}