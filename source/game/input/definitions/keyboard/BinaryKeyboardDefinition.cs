using System;
using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input.Keyboard.Definitions {
    public class BinaryKeyboardDefinition : IInputDefinition {
        public Keys Keys;
        public KeyPollType PollType;
        private float _current = 0;

        public float Test() {
            _current = KeyboardHelper.Poll(Keys, PollType) ? 1 : 0;
            return _current;
        }
    }
}