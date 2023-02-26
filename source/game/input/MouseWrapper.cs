using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Input {
    public class MouseWrapper {
        private static MouseStateExtended _prev;
        private static MouseStateExtended _curr;

        public static void Update() {
            _prev = _curr;
            _curr = MouseExtended.GetState();
        }

        // Was the button up in the last state, and down in the current?
        public static bool GetMouseDown(MouseButton button) {
            return _prev.IsButtonUp(button) && _curr.IsButtonDown(button);
        }

        // Was the button down in the last state, and up in the current?
        public static bool GetMouseUp(MouseButton button) {
            return _prev.IsButtonDown(button) && _curr.IsButtonUp(button);
        }

        // Is the button down in the current state?
        public static bool GetMouseHeld(MouseButton button) {
            return _curr.IsButtonDown(button);
        }

        // Get the position of the mouse pointer on the SCREEN, not the VIEWPORT.
        public static Point GetPosition() {
            return _curr.Position;
        }

        // Get the amount that the mouse has scrolled.
        public static int GetScrollDelta() {
            int delta = _curr.ScrollWheelValue - _prev.ScrollWheelValue;
            return delta == 0 ? 0 : (delta < 0 ? -1 : 1);
        }
    }
}