using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

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

        private TextureAtlas _patches;
        private int _patchSize;
        public Image MakePatches(int size) {
            if (_texture != null) {
                _patchSize = size;
                _patches = TextureAtlas.Create("patches", _texture, _patchSize, _patchSize);
            }
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_patches != null) ImageUtilities.DrawPatched(spriteBatch, GetRectangle(), _patches, _patchSize, _color);
            else spriteBatch.Draw(GetTexture(spriteBatch), GetRectangle(), null, _color);
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