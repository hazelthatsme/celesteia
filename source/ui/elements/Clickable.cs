using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace Celesteia.UI.Elements {
    public class Clickable : Element, IClickable
    {
        private bool _clicked;

        public override void OnMouseOut() {
            _clicked = false;
            base.OnMouseOut();
        }
        public virtual void OnMouseDown(MouseButton button, Point position) => _clicked = true;
        public virtual void OnMouseUp(MouseButton button, Point position) => _clicked = false;

        public bool GetClicked() => _clicked;
    }

    public delegate void HoverEvent();
    public delegate void ClickEvent(MouseButton button, Point position);
}