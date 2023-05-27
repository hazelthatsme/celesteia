namespace Celesteia.Game.Input.Conditions {
    public interface ICondition<T> {
        T Poll();
    }
}