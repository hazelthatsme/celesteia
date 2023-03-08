using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input {
    public interface IInputDefinition {
        float Test();
        InputType GetInputType();
    }

    public interface IInputDefinition<T> : IInputDefinition {
        T GetPositive();
        T GetNegative();
    }

    public enum InputType {
        Keyboard, Gamepad
    }

    public class KeyDefinition : IInputDefinition<Keys?> {
        public InputType GetInputType() => InputType.Keyboard;
        public readonly string Action;
        private readonly Keys? _negative;
        private readonly Keys? _positive;
        private float _current;
        private KeyDetectType _type;

        public KeyDefinition(string action, Keys? negative, Keys? positive, KeyDetectType type) {
            Action = action;
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
                    return KeyboardWrapper.GetKeyHeld(_keys.Value);
                case KeyDetectType.Down:
                    return KeyboardWrapper.GetKeyDown(_keys.Value);
                case KeyDetectType.Up:
                    return KeyboardWrapper.GetKeyUp(_keys.Value);
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