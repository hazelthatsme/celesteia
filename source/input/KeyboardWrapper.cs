using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celestia.GameInput {
    public class KeyboardWrapper {
        private KeyboardState currentState;
        private KeyboardState previousState;

        public void Update() {
            UpdateState();
        }

        private void UpdateState() {
            previousState = currentState;
            currentState = Keyboard.GetState();
        }

        public bool GetAnyKey() {
            return GetAnyKey(currentState);
        }

        public bool GetKeyDown(Keys keys) {
            return GetKeyHeld(currentState, keys) && !GetKeyHeld(previousState, keys);
        }

        public bool GetKeyUp(Keys keys) {
            return !GetKeyHeld(currentState, keys) && GetKeyHeld(previousState, keys);
        }

        private static bool GetAnyKey(KeyboardState state) {
            return state.GetPressedKeyCount() > 0;
        }

        private static bool GetKeyHeld(KeyboardState state, Keys keys) {
            return state.IsKeyDown(keys);
        }
    }
}