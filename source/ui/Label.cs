using System.Diagnostics;
using Celestia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.UI {
    public class Label : IElement {
        private Rect rect = Rect.AbsoluteZero;

        private Texture2D background;

        public string text = "";

        public TextAlignment textAlignment = TextAlignment.Left;

        private FontType font;

        public Label(Rect rect, Texture2D background, string text, TextAlignment alignment, FontType font) {
            this.rect = rect;
            this.background = background;
            this.text = text;
            this.textAlignment = alignment;
            this.font = font;
        }

        public Label(Rect rect, string text, TextAlignment alignment, FontType font) : this(rect, null, text, alignment, font) {}

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the label's background, if present.
            if (background != null) spriteBatch.Draw(GetTexture(spriteBatch), rect.ToXnaRectangle(), null, Color.White);

            // Credit for text alignment: https://stackoverflow.com/a/10263903
            float targetSize = 24f;

            // Measure the text's size from the sprite font.
            Vector2 size = font.Font.MeasureString(text) * font.Scale(targetSize);
            
            // Get the origin point at the center.
            Vector2 origin = 0.5f * size;

            if (textAlignment.HasFlag(TextAlignment.Left))
                origin.X += rect.Width.Resolve() / 2f - size.X / 2f;

            if (textAlignment.HasFlag(TextAlignment.Right))
                origin.X -= rect.Width.Resolve() / 2f - size.X / 2f;

            if (textAlignment.HasFlag(TextAlignment.Top))
                origin.Y += rect.Height.Resolve() / 2f - size.Y / 2f;

            if (textAlignment.HasFlag(TextAlignment.Bottom))
                origin.Y -= rect.Height.Resolve() / 2f - size.Y / 2f;

            spriteBatch.DrawString(font.Font, text, rect.GetCenter(), Color.White, 0f, origin, font.Scale(targetSize), SpriteEffects.None, 0f);
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            return this.background;
        }

        public Rect GetRect()
        {
            return this.rect;
        }

        public void SetTexture(Texture2D background)
        {
            this.background = background;
        }

        public void SetRect(Rect rect)
        {
            this.rect = rect;
        }
    }
}