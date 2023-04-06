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
}