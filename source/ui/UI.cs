using Celesteia.Resources.Collections;
using Celesteia.UI;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia {
    public static class UIReferences {
        public static GameWindow gameWindow;
        public static bool GUIEnabled = true;
        public static int Scaling = 3;
    }

    public static class TextUtilities {
        public static void DrawAlignedText(SpriteBatch spriteBatch, Rectangle rect, FontType font, string text, Color color, TextAlignment textAlignment, float targetSize) {
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

        public static void DrawAlignedText(SpriteBatch spriteBatch, Rectangle rect, TextProperties textProperties) {
            DrawAlignedText(spriteBatch, rect, textProperties.GetFont(), textProperties.GetText(), textProperties.GetColor(), textProperties.GetAlignment(), textProperties.GetFontSize());
        }
    }

    public static class ImageUtilities {
        private static int xLeft(Rectangle rectangle, int patchSize) => rectangle.X;
        private static int xMiddle(Rectangle rectangle, int patchSize) => rectangle.X + (patchSize * UIReferences.Scaling);
        private static int xRight(Rectangle rectangle, int patchSize) => rectangle.X + rectangle.Width - (patchSize * UIReferences.Scaling);
        private static int yTop(Rectangle rectangle, int patchSize) => rectangle.Y;
        private static int yMiddle(Rectangle rectangle, int patchSize) => rectangle.Y + (patchSize * UIReferences.Scaling);
        private static int yBottom(Rectangle rectangle, int patchSize) => rectangle.Y + rectangle.Height - (patchSize * UIReferences.Scaling);

        public static void DrawPatched(SpriteBatch spriteBatch, Rectangle rectangle, TextureAtlas patches, int patchSize, Color color) {
            int y;
            int scaled = patchSize * UIReferences.Scaling;

            // Top
            y = yTop(rectangle, patchSize);
            {
                spriteBatch.Draw(patches.GetRegion(0), new Rectangle(xLeft(rectangle, patchSize), y, scaled, scaled), color); // Top left
                spriteBatch.Draw(patches.GetRegion(1), new Rectangle(xMiddle(rectangle, patchSize), y, rectangle.Width - (2 * scaled), scaled), color); // Top center
                spriteBatch.Draw(patches.GetRegion(2), new Rectangle(xRight(rectangle, patchSize), y, scaled, scaled), color); // Top right
            }

            // Center
            y = yMiddle(rectangle, patchSize);
            {
                spriteBatch.Draw(patches.GetRegion(3), new Rectangle(xLeft(rectangle, patchSize), y, scaled, rectangle.Height - (2 * scaled)), color); // Left center
                spriteBatch.Draw(patches.GetRegion(4), new Rectangle(xMiddle(rectangle, patchSize), y, rectangle.Width - (2 * scaled), rectangle.Height - (2 * scaled)), color); // Center
                spriteBatch.Draw(patches.GetRegion(5), new Rectangle(xRight(rectangle, patchSize), y, scaled, rectangle.Height - (2 * scaled)), color); // Right center
            }

            // Bottom
            y = yBottom(rectangle, patchSize);
            {
                spriteBatch.Draw(patches.GetRegion(6), new Rectangle(xLeft(rectangle, patchSize), y, scaled, scaled), color); // Bottom left
                spriteBatch.Draw(patches.GetRegion(7), new Rectangle(xMiddle(rectangle, patchSize), y, rectangle.Width - (2 * scaled), scaled), color); // Bottom center
                spriteBatch.Draw(patches.GetRegion(8), new Rectangle(xRight(rectangle, patchSize), y, scaled, scaled), color); // Bottom right
            }
        }
    }
}