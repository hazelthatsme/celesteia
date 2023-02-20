using System;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI {
    public class Label : IElement {
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

        private Texture2D background;

        public string text = "";

        public TextAlignment textAlignment = TextAlignment.Left;

        private FontType font;

        public Label(Rect rect, Texture2D background, string text, TextAlignment alignment, FontType font) {
            _rect = rect;
            this.background = background;
            this.text = text;
            this.textAlignment = alignment;
            this.font = font;
        }

        public Label(Rect rect, string text, TextAlignment alignment, FontType font) : this(rect, null, text, alignment, font) {}

        public void Update(GameTime gameTime) {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the label's background, if present.
            if (background != null) spriteBatch.Draw(GetTexture(spriteBatch), GetRectangle(), null, Color.White);

            TextUtilities.DrawAlignedText(spriteBatch, font, text, textAlignment, GetRectangle(), 12f);
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            return this.background;
        }

        public void SetTexture(Texture2D background)
        {
            this.background = background;
        }
    }
}