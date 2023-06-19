using System;
using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input.Definitions.Keyboard {
    public class BinaryKeyboardDefinition : IBinaryInputDefinition {
        public Keys Keys;
        public InputPollType PollType;

        public bool Test() => KeyboardHelper.Poll(Keys, PollType);
    }
}