namespace Celesteia.Game.Input {
    public interface IInputDefinition {
        float Test();
    }

    public interface IInputDefinition<T> : IInputDefinition {
        T GetPositive();
        T GetNegative();
    }

    public enum InputType {
        Keyboard, Gamepad
    }
}