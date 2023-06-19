using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input.Definitions.Keyboard {
    public class TrinaryKeyboardDefinition : IFloatInputDefinition {
        public Keys Negative;
        public Keys Positive;
        public InputPollType PollType;

        private float _current = 0;

        public float Test() {
            _current =
                (KeyboardHelper.Poll(Negative, PollType) ? -1 : 0) +
                (KeyboardHelper.Poll(Positive, PollType) ? 1 : 0);
            return _current;
        }
    }
}