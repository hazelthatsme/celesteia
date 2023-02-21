using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Input {
    public class KeyboardWrapper {
        private static KeyboardStateExtended state;

        public static void Update() {
            state = KeyboardExtended.GetState();
        }

        public static bool GetAnyKey() {
            return !state.IsKeyDown(Keys.F3) && !state.IsKeyDown(Keys.F11) && state.GetPressedKeys().Length > 0;
        }

        public static bool GetKeyDown(Keys keys) {
            return state.WasKeyJustUp(keys) && state.IsKeyDown(keys);
        }

        public static bool GetKeyUp(Keys keys) {
            return state.WasKeyJustDown(keys) && state.IsKeyUp(keys);
        }

        public static bool GetKeyHeld(Keys keys) {
            return state.IsKeyDown(keys);
        }
    }
}