using Celesteia.Game.Input.Definitions;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Input {
    public static class MouseHelper {
        private static MouseStateExtended _prev;
        private static MouseStateExtended _curr;

        public static int ScrollDelta => _curr.ScrollWheelValue - _prev.ScrollWheelValue;

        // Get the position of the mouse pointer on the SCREEN, not the VIEWPORT.
        public static Point Position => _curr.Position;

        public static void Update() {
            _prev = _curr;
            _curr = MouseExtended.GetState();
        }

        // Was true, now true.
        public static bool Held(MouseButton button) => _prev.IsButtonDown(button) && _curr.IsButtonDown(button);
        // Is true.
        public static bool IsDown(MouseButton button) => _curr.IsButtonDown(button);
        // Was false, now true.
        public static bool Pressed(MouseButton button) => !_prev.IsButtonDown(button) && _curr.IsButtonDown(button);
        // Was true, now false.
        public static bool Released(MouseButton button) => _prev.IsButtonDown(button) && !_curr.IsButtonDown(button);

        public static bool Poll(MouseButton button, InputPollType type) => type switch {
            InputPollType.Held => Held(button),
            InputPollType.IsDown => IsDown(button),
            InputPollType.Pressed => Pressed(button),
            InputPollType.Released => Released(button),
            _ => false
        };
    }
}