using System.Collections.Generic;
using Celesteia.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;

namespace Celesteia.UI.Elements {
    public class Container : Element, IContainer
    {
        private List<IElement> Children;

        public Container(Rect rect) {
            SetRect(rect);
            Children = new List<IElement>();
        }

        public void AddChild(IElement element) {
            Children.Add(element);
            element.SetParent(this);
        }

        public List<IElement> GetChildren() => Children;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Children.ForEach(element => { if (element.GetEnabled()) element.Draw(spriteBatch); });
        }

        private Point _mousePosition;

        public override void Update(GameTime gameTime, out bool clickedAnything)
        {
            clickedAnything = false;
            if (!UIReferences.GUIEnabled) return;

            foreach (IElement element in Children) {
                element.Update(gameTime, out clickedAnything);
            }

            _mousePosition = MouseHelper.Position;

            if (MouseHelper.Pressed(MouseButton.Left))  clickedAnything = ResolveMouseDown(MouseButton.Left);
            else if (MouseHelper.Pressed(MouseButton.Right))  clickedAnything = ResolveMouseDown(MouseButton.Right);

            if (MouseHelper.Released(MouseButton.Left)) clickedAnything = ResolveMouseUp(MouseButton.Left);
            else if (MouseHelper.Released(MouseButton.Right)) clickedAnything = ResolveMouseUp(MouseButton.Right);

            ResolveMouseOver();
        }

        public bool ResolveMouseDown(MouseButton button) {
            bool clicked = false;
            Children.FindAll(x => x is Clickable).ForEach(element => {
                if (!element.GetEnabled()) return;
                Clickable clickable = element as Clickable;

                if (clicked = clickable.GetRectangle().Contains(_mousePosition))
                    clickable.OnMouseDown(button, _mousePosition);
            });
            return clicked;
        }

        public bool ResolveMouseUp(MouseButton button) {
            bool clicked = false;
            Children.FindAll(x => x is Clickable).ForEach(element => {
                if (!element.GetEnabled()) return;
                Clickable clickable = element as Clickable;

                if (clickable.GetRectangle().Contains(_mousePosition)) {
                    clicked = true;
                    clickable.OnMouseUp(button, _mousePosition);
                }
            });
            return clicked;
        }

        public void ResolveMouseOver() {
            Children.ForEach(element => {
                if (!element.GetEnabled()) return;
                bool over = element.GetRectangle().Contains(_mousePosition);
                if (over && !element.GetMouseOver()) {
                    element.OnMouseIn();
                } else if (!over && element.GetMouseOver()) element.OnMouseOut();
            });
        }

        public void Dispose() {
            Children.Clear();
        }
    }
}