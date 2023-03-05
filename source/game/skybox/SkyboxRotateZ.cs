namespace Celesteia.Game.Skybox {
    public class SkyboxRotateZ {
        public float _magnitude = 1f;
        // Amount to rotate by.
        public float Magnitude { get { return _magnitude; } }

        // Current rotation.
        public float Current = 0f;

        public SkyboxRotateZ(float magnitude) {
            _magnitude = magnitude;

            // Offset the starting rotation.
            Current = _magnitude * 256f;
        }
    }
}