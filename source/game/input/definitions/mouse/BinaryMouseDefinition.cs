using MonoGame.Extended.Input;

namespace Celesteia.Game.Input.Definitions.Mouse {
    public class BinaryMouseDefinition : IBinaryInputDefinition {
        public MouseButton Button;
        public InputPollType PollType;

        public bool Test() => MouseHelper.Poll(Button, PollType);
    }
}