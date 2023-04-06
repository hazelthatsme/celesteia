using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Input {
    public class KeyboardWrapper {
        private KeyboardStateExtended _prev;
        private KeyboardStateExtended _curr;

        public void Update() {
            _prev = _curr;
            _curr = KeyboardExtended.GetState();
        }

        // Is any key (excluding F3 and F11) down in the current state?
        public bool GetAnyKey() {
            return !_curr.IsKeyDown(Keys.F3) && !_curr.IsKeyDown(Keys.F11) && _curr.GetPressedKeys().Length > 0;
        }

        // Was the key up in the last state, and down in the current?
        public bool GetKeyDown(Keys keys) {
            return _prev.IsKeyUp(keys) && _curr.IsKeyDown(keys);
        }

        // Was the key down in the last state, and up in the current?
        public bool GetKeyUp(Keys keys) {
            return _prev.IsKeyDown(keys) && _curr.IsKeyUp(keys);
        }

        // Is the key down in the current state?
        public bool GetKeyHeld(Keys keys) {
            return _curr.IsKeyDown(keys);
        }
    }

    public class KeyDefinition : IInputDefinition<Keys?> {
        public InputType GetInputType() => InputType.Keyboard;
        private KeyboardWrapper _wrapper;
        public readonly string Name;
        private readonly Keys? _negative;
        private readonly Keys? _positive;
        private float _current;
        private KeyDetectType _type;

        public KeyDefinition(KeyboardWrapper wrapper, string name, Keys? negative, Keys? positive, KeyDetectType type) {
            _wrapper = wrapper;
            Name = name;
            _negative = negative;
            _positive = positive;
            _current = 0;
            _type = type;
        }

        public float Test() {
            bool negativeHeld = _negative.HasValue && Detect(_negative.Value);
            bool positiveHeld = _positive.HasValue && Detect(_positive.Value);

            _current = (negativeHeld ? -1 : 0) + (positiveHeld ? 1 : 0);
            return _current;
        }

        public bool Detect(Keys? _keys) {
            switch (_type) {
                case KeyDetectType.Held:
                    return _wrapper.GetKeyHeld(_keys.Value);
                case KeyDetectType.Down:
                    return _wrapper.GetKeyDown(_keys.Value);
                case KeyDetectType.Up:
                    return _wrapper.GetKeyUp(_keys.Value);
            }

            return false;
        }

        public Keys? GetPositive() => _positive;
        public Keys? GetNegative() => _negative;
    }

    public enum KeyDetectType {
        Held, Down, Up
    }
}