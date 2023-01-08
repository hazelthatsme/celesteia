using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celestia.GameInput {
    public static class Input {
        public static KeyboardWrapper Keyboard { get; private set; }
        public static MouseWrapper Mouse { get; private set; }
        public static GamepadWrapper Gamepad { get; private set; }

        public static void Initialize() {
            Keyboard = new KeyboardWrapper();
            Mouse = new MouseWrapper();
            Gamepad = new GamepadWrapper();
        }

        public static void Update() {
            Keyboard.Update();
            Mouse.Update();
            Gamepad.Update();
        }

        public static bool GetAny() {
            return Keyboard.GetAnyKey() || Gamepad.GetAnyButton();
        }
    }
}