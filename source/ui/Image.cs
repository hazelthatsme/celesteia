using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI {
    public class Image : IElement
    {
        private Rect rect;
        private Texture2D texture;
        public Color color;
        public float depth;
        public ImageFitMode imageFitMode;

        public Image(Rect rect, Texture2D texture, Color color, float depth, ImageFitMode imageFitMode) {
            this.rect = rect;
            this.texture = texture;
            this.color = color;
            this.depth = depth;
            this.imageFitMode = imageFitMode;
        }

        public Image(Rect rect, Texture2D texture, Color color, float depth) : this (rect, texture, color, depth, ImageFitMode.Fill) {}

        public Image(Rect rect, Texture2D texture, float depth) : this (rect, texture, Color.White, depth) {}

        public Image(Rect rect, Texture2D texture, Color color) : this (rect, texture, color, 0f) {}

        public Image(Rect rect, Texture2D texture) : this (rect, texture, Color.White, 0f) {}

        public Image(Rect rect, Color color) : this (rect, null, color, 0f) {}

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D sampledTexture = GetTexture(spriteBatch);
            spriteBatch.Draw(sampledTexture, rect.ToXnaRectangle(), color);
        }

        public Rect GetRect()
        {
            return this.rect;
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (this.texture == null) {
                // Make a new texture.
                this.texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);

                // Set the default texture to a configured color if present, otherwise it's black.
                this.texture.SetData(new[] { Color.Black });
            }

            return this.texture;
        }

        public void SetRect(Rect rect) => this.rect = rect;

        public void SetTexture(Texture2D texture) => this.texture = texture;

        public void OnMouseIn() { }

        public void OnMouseOut() { }
    }

    public enum ImageFitMode {
        Fill,
        Contain,
        Cover
    }
}