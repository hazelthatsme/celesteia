using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celestia {
    public static class Input {
        public static MouseButtonState MouseButtons { get; private set; }
        public static Point MousePosition { get; private set; }

        private static MouseState mouseState;

        public static void Update() {
            UpdateMouse();
        }

        private static void UpdateMouse() {
            mouseState = Mouse.GetState();

            MouseButtons = (MouseButtonState)(((int) mouseState.LeftButton) + (2 * (int) mouseState.RightButton));
            MousePosition = mouseState.Position;
        }
    }

    public enum MouseButtonState {
        None = 0,
        Left = 1,
        Right = 2,
        Both = 3
    }
}