using System.Collections.Generic;
using System.Linq;
using Celesteia.Game.Input.Definitions;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Input {
    public static class KeyboardHelper {
        private static KeyboardStateExtended _prev;
        private static KeyboardStateExtended _curr;

        public static void Update() {
            _prev = _curr;
            _curr = KeyboardExtended.GetState();
        }

        // Is any key (excluding the exemptions) down in the current state?
        public static List<Keys> exemptedFromAny = new List<Keys> {
            { Keys.F3 }, { Keys.F11 }
        };

        private static Keys[] pressed;
        public static bool AnyKey() {
            pressed = _curr.GetPressedKeys();
            bool anyKey = pressed.Length > 0;
            
            if (anyKey)
                for (int i = 0; i < exemptedFromAny.Count; i++)
                    anyKey = !pressed.Contains(exemptedFromAny[i]);

            return anyKey;
        }

        // Was true, now true.
        public static bool Held(Keys keys) => _prev.IsKeyDown(keys) && _curr.IsKeyDown(keys);
        // Is true.
        public static bool IsDown(Keys keys) => _curr.IsKeyDown(keys);
        // Was false, now true.
        public static bool Pressed(Keys keys) => !_prev.IsKeyDown(keys) && _curr.IsKeyDown(keys);
        // Was true, now false.
        public static bool Released(Keys keys) => _prev.IsKeyDown(keys) && !_curr.IsKeyDown(keys);

        public static bool Poll(Keys keys, InputPollType type) => type switch {
            InputPollType.Held => Held(keys),
            InputPollType.IsDown => IsDown(keys),
            InputPollType.Pressed => Pressed(keys),
            InputPollType.Released => Released(keys),
            _ => false
        };
    }

    
}