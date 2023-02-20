using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;

namespace Celesteia.UI {
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
            Children.ForEach(element => element.Draw(spriteBatch));
        }

        private Point _mousePosition;

        public override void Update(GameTime gameTime)
        {
            Children.ForEach(element => element.Update(gameTime));

            if (!Game.GUIEnabled) return;

            _mousePosition = MouseWrapper.GetPosition();

            if (MouseWrapper.GetMouseDown(MouseButton.Left)) ResolveMouseDown(MouseButton.Left);
            if (MouseWrapper.GetMouseUp(MouseButton.Left)) ResolveMouseUp(MouseButton.Left);

            ResolveMouseOver();
        }

        public void ResolveMouseDown(MouseButton button) {
            Children.FindAll(x => x is IClickable).ForEach(element => {
                IClickable clickable = element as IClickable;

                if (clickable.GetRectangle().Contains(_mousePosition)) {
                    clickable.OnMouseDown(button, _mousePosition);
                }
            });
        }

        public void ResolveMouseUp(MouseButton button) {
            Children.FindAll(x => x is IClickable).ForEach(element => {
                IClickable clickable = element as IClickable;

                if (clickable.GetRectangle().Contains(_mousePosition)) {
                    clickable.OnMouseUp(button, _mousePosition);
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
    }
}