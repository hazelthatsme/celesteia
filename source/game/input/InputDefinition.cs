using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input {
    public interface IInputDefinition {
        float Test();
    }

    public class KeyDefinition : IInputDefinition {
        private readonly Keys? _negative;
        private readonly Keys? _positive;
        private float _current;

        public KeyDefinition(Keys? negative, Keys? positive) {
            _negative = negative;
            _positive = positive;
            _current = 0;
        }

        public float Test() {
            bool negativeHeld = _negative.HasValue && KeyboardWrapper.GetKeyHeld(_negative.Value);
            bool positiveHeld = _positive.HasValue && KeyboardWrapper.GetKeyHeld(_positive.Value);

            _current = (negativeHeld ? -1 : 0) + (positiveHeld ? 1 : 0);
            return _current;
        }
    }
}