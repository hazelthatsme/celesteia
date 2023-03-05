using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Input {
    public class KeyboardWrapper {
        private static KeyboardStateExtended _prev;
        private static KeyboardStateExtended _curr;

        public static void Update() {
            _prev = _curr;
            _curr = KeyboardExtended.GetState();
        }

        // Is any key (excluding F3 and F11) down in the current state?
        public static bool GetAnyKey() {
            return !_curr.IsKeyDown(Keys.F3) && !_curr.IsKeyDown(Keys.F11) && _curr.GetPressedKeys().Length > 0;
        }

        // Was the key up in the last state, and down in the current?
        public static bool GetKeyDown(Keys keys) {
            return _prev.IsKeyUp(keys) && _curr.IsKeyDown(keys);
        }

        // Was the key down in the last state, and up in the current?
        public static bool GetKeyUp(Keys keys) {
            return _prev.IsKeyDown(keys) && _curr.IsKeyUp(keys);
        }

        // Is the key down in the current state?
        public static bool GetKeyHeld(Keys keys) {
            return _curr.IsKeyDown(keys);
        }
    }
}