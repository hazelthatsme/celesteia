using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements {
    public class Image : Element {
        private Texture2D _texture;
        public Color _color;

        public Image(Rect rect) {
            SetRect(rect);
        }

        public Image SetTexture(Texture2D texture) {
            _texture = texture;
            return this;
        }

        public Image SetColor(Color color) {
            _color = color;
            return this;
        }

        public Image SetPivotPoint(Vector2 pivot) {
            SetPivot(pivot);
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GetTexture(spriteBatch), GetRectangle(), _color);
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null) {
                // Make a new texture.
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);

                // Set the default texture to one gray pixel.
                _texture.SetData(new[] { Color.Gray });
            }

            return _texture;
        }
    }
}