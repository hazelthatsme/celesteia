using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Input {
    public class MouseWrapper {
        private MouseStateExtended _prev;
        private MouseStateExtended _curr;

        public int ScrollDelta => _curr.ScrollWheelValue - _prev.ScrollWheelValue;

        // Get the position of the mouse pointer on the SCREEN, not the VIEWPORT.
        public Point Position => _curr.Position;

        public void Update() {
            _prev = _curr;
            _curr = MouseExtended.GetState();
        }

        // Was the button up in the last state, and down in the current?
        public bool GetMouseDown(MouseButton button) => _prev.IsButtonUp(button) && _curr.IsButtonDown(button);

        // Was the button down in the last state, and up in the current?
        public bool GetMouseUp(MouseButton button) => _prev.IsButtonDown(button) && _curr.IsButtonUp(button);

        // Is the button down in the current state?
        public bool GetMouseHeld(MouseButton button) => _curr.IsButtonDown(button);
    }
}