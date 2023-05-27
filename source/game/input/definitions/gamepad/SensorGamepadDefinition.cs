namespace Celesteia.Game.Input.Definitions.Gamepad {
    public class SensorGamepadDefinition : IFloatInputDefinition {
        public GamePadSensor Sensor;
        public InputPollType PollType;
        private float _current = 0;

        public float Test() {
            _current = GamepadHelper.PollSensor(Sensor);
            return _current;
        }
    }
}