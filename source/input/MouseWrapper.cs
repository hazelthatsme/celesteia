using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;

namespace Celesteia.GameInput {
    public class MouseWrapper {
        private static MouseStateExtended state;

        public static void Update() {
            state = MouseExtended.GetState();
        }

        public static bool GetMouseDown(MouseButton button) {
            return state.WasButtonJustUp(button);
        }

        public static bool GetMouseUp(MouseButton button) {
            return state.WasButtonJustDown(button);
        }

        public static bool GetMouseHeld(MouseButton button) {
            return state.IsButtonDown(button);
        }

        public static Point GetPosition() {
            return state.Position;
        }
    }
}