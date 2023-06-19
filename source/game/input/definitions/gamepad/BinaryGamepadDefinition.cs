using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input.Definitions.Gamepad {
    public class BinaryGamepadDefinition : IBinaryInputDefinition {
        public Buttons Buttons;
        public InputPollType PollType;

        public bool Test() => GamepadHelper.PollButtons(Buttons, PollType);
    }
}