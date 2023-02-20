using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI {
    public class Image : IElement {
        private IContainer _parent;
        public IContainer GetParent() => _parent;
        public void SetParent(IContainer parent) => _parent = parent;
        public Rect GetRect() => _rect;
        public void SetRect(Rect rect) => _rect = rect;
        public void OnMouseIn() { }
        public void OnMouseOut() { }
        private Vector2 _pivot;
        public Vector2 GetPivot() => _pivot;
        public void SetPivot(Vector2 pivot) => _pivot = pivot;

        public Rectangle GetRectangle() {
            Rectangle r = GetRect().ResolveRectangle();

            if (GetParent() != null) {
                r.X += GetParent().GetRectangle().X;
                r.Y += GetParent().GetRectangle().Y;
            }

            r.X -= (int)Math.Round(_pivot.X * r.Width);
            r.Y -= (int)Math.Round(_pivot.Y * r.Height);
            
            return r;
        }
        
        private Rect _rect;
        private Texture2D _texture;
        public Color _color;

        public Image(Rect rect) {
            _rect = rect;
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
            _pivot = pivot;
            return this;
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
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

    public enum ImageFitMode {
        Fill,
        Contain,
        Cover
    }
}