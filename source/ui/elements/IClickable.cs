using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace Celesteia.UI.Elements {
    public interface IClickable : IElement {
        void OnMouseDown(MouseButton button, Point position);
        void OnMouseUp(MouseButton button, Point position);
    }
}