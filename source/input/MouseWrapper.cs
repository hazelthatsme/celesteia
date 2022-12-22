using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celestia.GameInput {
    public class MouseWrapper {
        private MouseState previousState;
        private MouseState currentState;

        public void Update() {
            UpdateState();
        }

        private void UpdateState() {
            previousState = currentState;
            currentState = Mouse.GetState();
        }

        public bool GetMouseDown(MouseButtons button) {
            return GetMouseHeld(currentState, button) && !GetMouseHeld(previousState, button);
        }

        public bool GetMouseHeld(MouseButtons button) {
            return GetMouseHeld(currentState, button);
        }

        public bool GetMouseUp(MouseButtons button) {
            return !GetMouseHeld(currentState, button) && GetMouseHeld(previousState, button);
        }

        public Point GetPosition() {
            return GetPosition(currentState);
        }

        private static bool GetMouseHeld(MouseState state, MouseButtons buttons) {
            MouseButtons b = GetStateButtons(state);
            return b.HasFlag(buttons);
        }

        private static MouseButtons GetStateButtons(MouseState state) {
            MouseButtons b = (MouseButtons) ((int) state.LeftButton * 1) + ((int) state.RightButton * 2) + ((int) state.MiddleButton * 4);
            return b;
        }
        
        private static Point GetPosition(MouseState state) {
            return state.Position;
        }
    }

    [Flags]
    public enum MouseButtons {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4
    }
}