using System;
using System.Collections.Generic;
using Celesteia.GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;

namespace Celesteia.UI {
    public class Container : IContainer
    {
        private Rect _rect;

        private List<IElement> Children;
        private IContainer Parent;
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

        public Container(Rect rect) {
            _rect = rect;
            Children = new List<IElement>();
        }

        public void AddChild(IElement element) {
            Children.Add(element);
            element.SetParent(this);
        }

        public List<IElement> GetChildren() => Children;

        public void SetParent(IContainer container) {
            Parent = container;
        }

        public IContainer GetParent()
        {
            return Parent;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Children.ForEach(element => element.Draw(spriteBatch));
        }

        private Point _mousePosition;

        public void Update(GameTime gameTime)
        {
            Children.ForEach(element => element.Update(gameTime));

            if (!Game.GUIEnabled) return;

            _mousePosition = MouseWrapper.GetPosition();

            if (MouseWrapper.GetMouseDown(MouseButton.Left)) {
                ResolveMouseClick(MouseButton.Left);
            }

            ResolveMouseOver();
        }

        public void ResolveMouseClick(MouseButton button) {
            Children.FindAll(x => x is IClickableElement).ForEach(element => {
                IClickableElement clickable = element as IClickableElement;

                if (clickable.GetRectangle().Contains(_mousePosition)) {
                    clickable.OnClick(_mousePosition);
                }
            });
        }

        public void ResolveMouseOver() {
            Children.ForEach(element => {
                if (element.GetRectangle().Contains(_mousePosition)) {
                    element.OnMouseIn();
                } else element.OnMouseOut();
            });
        }

        public Rect GetRect() => _rect;

        public void SetRect(Rect rect) => _rect = rect;

        public void OnMouseIn() {}
        public void OnMouseOut() {}
    }
}