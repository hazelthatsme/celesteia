using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI {
    public class Element : IElement
    {
        private bool _enabled;
        private Rect _rect;
        private bool _isMouseOver;
        private IContainer _parent;
        private Vector2 _pivot;

        public virtual bool GetEnabled() => _enabled;
        public virtual IContainer GetParent() => _parent;
        public virtual Vector2 GetPivot() => _pivot;
        public virtual Rect GetRect() => _rect;

        // Get element rectangle with pivot.
        public virtual Rectangle GetRectangle() {
            Rectangle r = GetRect().ResolveRectangle();

            if (GetParent() != null) {
                r.X += GetParent().GetRectangle().X;
                r.Y += GetParent().GetRectangle().Y;
            }

            r.X -= (int)Math.Round(_pivot.X * r.Width);
            r.Y -= (int)Math.Round(_pivot.Y * r.Height);
            
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
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}