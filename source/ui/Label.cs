using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI {
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

            TextUtilities.DrawAlignedText(spriteBatch, font, text, textAlignment, rect, 12f);
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

        public void OnMouseIn() {}
        public void OnMouseOut() {}
    }
}