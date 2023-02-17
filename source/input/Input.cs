using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;

namespace Celesteia.GameInput {
    public static class Input {
        public static GamepadWrapper Gamepad { get; private set; }

        public static void Initialize() {
            Gamepad = new GamepadWrapper();
        }

        public static void Update() {
            KeyboardWrapper.Update();
            MouseWrapper.Update();
            Gamepad.Update();
        }

        public static bool GetAny() {
            return KeyboardWrapper.GetAnyKey() || Gamepad.GetAnyButton();
        }
    }
}