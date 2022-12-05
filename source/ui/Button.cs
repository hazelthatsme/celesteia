using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.UI {
    public class Button {
        public Rect rect;

        public delegate void OnClick(Point position);
        private OnClick onClick;

        private Texture2D texture;

        public string Text { get; private set; }

        public TextAlignment alignment { get; private set; }
        
        public Button(Rect rect, OnClick onClick, string text, TextAlignment alignment) {
            this.rect = rect;
            this.onClick = onClick;
            this.Text = text;
            this.alignment = alignment;

            this.texture = GetTexture();
        }

        public Button(Rect rect, OnClick onClick, string text) : this (rect, null, text, TextAlignment.Left & TextAlignment.Center) { }
        public Button(Rect rect, OnClick onClick) : this (rect, onClick, "") { }
        public Button(Rect rect) : this (rect, null) { }

        public Texture2D GetTexture() {
            if (this.texture == null) {
                this.texture = new Texture2D(Game.GetSpriteBatch().GraphicsDevice, 1, 1);
                this.texture.SetData(new[] { Color.Gray });
            }

            return this.texture;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont) {
            // Draw the button's texture.
            spriteBatch.Draw(GetTexture(), rect.ToXnaRectangle(), null, Color.White);

            // Credit for text alignment: https://stackoverflow.com/a/10263903

            // Measure the text's size from the sprite font.
            Vector2 size = spriteFont.MeasureString(Text);
            
            // Get the origin point at the center.
            Vector2 origin = 0.5f * size;

            if (alignment.HasFlag(TextAlignment.Left))
                origin.X += rect.Width / 2f - size.X / 2f;

            if (alignment.HasFlag(TextAlignment.Right))
                origin.X -= rect.Width / 2f - size.X / 2f;

            if (alignment.HasFlag(TextAlignment.Top))
                origin.Y += rect.Height / 2f - size.Y / 2f;

            if (alignment.HasFlag(TextAlignment.Bottom))
                origin.Y -= rect.Height / 2f - size.Y / 2f;

            spriteBatch.DrawString(spriteFont, Text, rect.GetCenter(), Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        public void Click(Point position) {
            onClick?.Invoke(position);
        }
    }
}