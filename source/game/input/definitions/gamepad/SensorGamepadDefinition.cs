using System;

namespace Celesteia.Game.Input.Definitions.Gamepad {
    public class SensorGamepadDefinition : IFloatInputDefinition {
        public GamePadSensor Sensor;
        private float _current = 0;

        public float Test() {
            _current = GamepadHelper.PollSensor(Sensor);
            return _current;
        }
    }
}