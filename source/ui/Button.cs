using System.Diagnostics;
using Celestia.Resources;
using Celestia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.UI {
    public class Button : IElement {
        private Rect rect = Rect.AbsoluteZero;

        public delegate void OnClick(Point position);
        private OnClick onClick = null;

        private Texture2D texture;

        public string text = "";

        public TextAlignment textAlignment = TextAlignment.Left;

        private FontType font;

        public Button(Rect rect, OnClick onClick, Texture2D texture, string text, TextAlignment alignment) {
            this.rect = rect;
            this.onClick = onClick;
            this.texture = texture;
            this.text = text;
            this.textAlignment = alignment;
            font = ResourceManager.Fonts.GetFontType("Hobo");
        }

        public void Click(Point position) {
            onClick?.Invoke(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button's texture.
            spriteBatch.Draw(GetTexture(spriteBatch), rect.ToXnaRectangle(), null, Color.White);

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
            if (this.texture == null) {
                this.texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                this.texture.SetData(new[] { Color.Gray });
            }

            return this.texture;
        }

        public void SetOnClick(OnClick onClick) {
            this.onClick = onClick;
        }

        public Rect GetRect()
        {
            return this.rect;
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void SetRect(Rect rect)
        {
            this.rect = rect;
        }
    }
}