using Celesteia.Resources.Types;
using Celesteia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia {
    public static class UIReferences {
        public static GameWindow gameWindow;
        public static int Scaling = 3;
    }

    public static class TextUtilities {
        public static void DrawAlignedText(SpriteBatch spriteBatch, FontType font, string text, Color color, TextAlignment textAlignment, Rectangle rect, float targetSize) {
            // Credit for text alignment: https://stackoverflow.com/a/10263903

            // Measure the text's size from the sprite font.
            Vector2 size = font.Font.MeasureString(text);
            
            // Get the origin point at the center.
            Vector2 origin = 0.5f * size;

            if (textAlignment.HasFlag(TextAlignment.Left))
                origin.X += rect.Width / 2f - size.X / 2f;

            if (textAlignment.HasFlag(TextAlignment.Right))
                origin.X -= rect.Width / 2f - size.X / 2f;

            if (textAlignment.HasFlag(TextAlignment.Top))
                origin.Y += rect.Height / 2f - size.Y / 2f;

            if (textAlignment.HasFlag(TextAlignment.Bottom))
                origin.Y -= rect.Height / 2f - size.Y / 2f;

            spriteBatch.DrawString(font.Font, text, new Vector2(rect.Center.X, rect.Center.Y), color, 0f, origin, font.Scale(targetSize), SpriteEffects.None, 0f);
        }
    }
}