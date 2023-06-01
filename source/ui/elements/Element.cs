using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements {
    public class Element : IElement
    {
        private bool _enabled = true;
        private Rect _rect;
        private bool _isMouseOver;
        private IContainer _parent;
        private Vector2 _pivot;

        public virtual bool GetEnabled() => _enabled && (_parent == null || _parent.GetEnabled());
        public virtual IContainer GetParent() => _parent;
        public virtual Vector2 GetPivot() => _pivot;
        public virtual Rect GetRect() => _rect;
        public virtual void MoveTo(Point point) {
            _rect.SetX(AbsoluteUnit.WithValue(point.X));
            _rect.SetY(AbsoluteUnit.WithValue(point.Y));
        }

        // Get element rectangle with pivot.
        public virtual Rectangle GetRectangle() {
            Rectangle r = GetRect().ResolveRectangle();

            if (GetParent() != null) {
                r.X += GetParent().GetRectangle().X;
                r.Y += GetParent().GetRectangle().Y;
            }

            r.X -= (int)MathF.Round(_pivot.X * r.Width);
            r.Y -= (int)MathF.Round(_pivot.Y * r.Height);
            
            return r;
        }

        // Interface setters.
        public virtual bool SetEnabled(bool value) => _enabled = value;
        public virtual void SetParent(IContainer container) => _parent = container;
        public virtual void SetPivot(Vector2 pivot) => _pivot = pivot;
        public virtual void SetRect(Rect rect) => _rect = rect;

        // Mouse functions
        public virtual void OnMouseIn() => _isMouseOver = true;
        public virtual void OnMouseOut() => _isMouseOver = false;
        public virtual bool GetMouseOver() => _isMouseOver;

        // Engine functions.
        public virtual void Update(GameTime gameTime, out bool clickedAnything) { clickedAnything = false; }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}