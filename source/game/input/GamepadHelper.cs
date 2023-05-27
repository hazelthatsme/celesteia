using System;
using Celesteia.Game.Input.Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celesteia.Game.Input {
    public static class GamepadHelper {
        private static GamePadState _prev;
        private static GamePadState _curr;

        public static void Update() {
            _prev = _curr;
            _curr = GamePad.GetState(0);
        }

        public static bool AnyButton() => !IsDown(Buttons.None);

        // Was true, now true.
        public static bool Held(Buttons buttons) => _prev.IsButtonDown(buttons) && _curr.IsButtonDown(buttons);
        // Is true.
        public static bool IsDown(Buttons buttons) => _curr.IsButtonDown(buttons);
        // Was false, now true.
        public static bool Pressed(Buttons buttons) => !_prev.IsButtonDown(buttons) && _curr.IsButtonDown(buttons);
        // Was true, now false.
        public static bool Released(Buttons buttons) => _prev.IsButtonDown(buttons) && !_curr.IsButtonDown(buttons);

        public static bool PollButtons(Buttons buttons, InputPollType type) => type switch {
            InputPollType.Held => Held(buttons),
            InputPollType.IsDown => IsDown(buttons),
            InputPollType.Pressed => Pressed(buttons),
            InputPollType.Released => Released(buttons),
            _ => false
        };

        public static float PollSensor(GamePadSensor sensor) => sensor switch {
            GamePadSensor.LeftThumbStickX => _curr.ThumbSticks.Left.X,
            GamePadSensor.LeftThumbStickY => _curr.ThumbSticks.Left.Y,
            GamePadSensor.LeftTrigger => _curr.Triggers.Left,
            GamePadSensor.RightThumbStickX => _curr.ThumbSticks.Right.X,
            GamePadSensor.RightThumbStickY => _curr.ThumbSticks.Right.Y,
            GamePadSensor.RightTrigger => _curr.Triggers.Right,
            _ => 0f
        };
    }

    public enum GamePadSensor {
        LeftThumbStickX,
        LeftThumbStickY,
        LeftTrigger,
        RightThumbStickX,
        RightThumbStickY,
        RightTrigger
    }
}