namespace Celesteia.Game.Skybox {
    public class SkyboxRotateZ {
        public float Magnitude { get { return _magnitude; } }
        public float _magnitude = 1f;
        public float Current = 0f;

        public SkyboxRotateZ(float magnitude) {
            _magnitude = magnitude;
            Current = _magnitude * 256f;
        }
    }
}