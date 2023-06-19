namespace Celesteia.Game.Input.Definitions {
    public interface IInputDefinition<T> {
        T Test();
    }

    public interface IBinaryInputDefinition : IInputDefinition<bool> {}
    public interface IFloatInputDefinition : IInputDefinition<float> {}

    public enum InputType {
        Keyboard, Gamepad
    }

    public enum InputPollType {
        IsDown, Held, Pressed, Released
    }
}