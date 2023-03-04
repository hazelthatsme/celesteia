using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input {
    public interface IInputDefinition {
        float Test();
    }

    public class KeyDefinition : IInputDefinition {
        private readonly Keys? _negative;
        private readonly Keys? _positive;
        private float _current;
        private KeyDetectType _type;

        public KeyDefinition(Keys? negative, Keys? positive, KeyDetectType type) {
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
    }

    public enum KeyDetectType {
        Held, Down, Up
    }
}