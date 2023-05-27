using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input.Keyboard.Definitions {
    public class TrinaryKeyboardDefinition : IInputDefinition {
        public Keys Negative;
        public Keys Positive;
        public KeyPollType PollType;

        private float _current = 0;

        public float Test() {
            _current =
                (KeyboardHelper.Poll(Negative, PollType) ? -1 : 0) +
                (KeyboardHelper.Poll(Positive, PollType) ? 1 : 0);
            return _current;
        }
    }
}