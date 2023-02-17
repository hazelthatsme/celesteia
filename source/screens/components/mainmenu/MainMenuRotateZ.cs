namespace Celesteia.Screens.Systems.MainMenu {
    public class MainMenuRotateZ {
        public float Magnitude { get { return _magnitude; } }
        public float _magnitude = 1f;
        public float Current = 0f;

        public MainMenuRotateZ(float magnitude) {
            _magnitude = magnitude;
            Current = _magnitude * 256f;
        }
    }
}