using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celestia {
    public static class Input {
        public static MouseButtonState MouseButtons { get; private set; }
        public static Point MousePosition { get; private set; }

        private static MouseState mouseState;
        private static KeyboardState currentKeyState;
        private static KeyboardState previousKeyState;

        private static GamePadState currentGamePadState;
        private static GamePadState previousGamePadState;

        public static void Update() {
            UpdateMouse();
            UpdateKeyboard();
            UpdateGamepad();
        }

        public static bool GetAnyKey() {
            return currentKeyState.GetPressedKeyCount() > 0;
        }

        public static bool GetKeyDown(Keys key) {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }

        public static bool GetKeyUp(Keys key) {
            return currentKeyState.IsKeyUp(key) && !previousKeyState.IsKeyUp(key);
        }

        public static bool GetKeyHeld(Keys key) {
            return currentKeyState.IsKeyDown(key);
        }

        private static void UpdateKeyboard() {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }

        private static void UpdateMouse() {
            mouseState = Mouse.GetState();

            MouseButtons = (MouseButtonState)(((int) mouseState.LeftButton) + (2 * (int) mouseState.RightButton));
            MousePosition = mouseState.Position;
        }

        public static bool GetButtonDown(Buttons button) {
            return currentGamePadState.IsButtonDown(button) && !previousGamePadState.IsButtonDown(button);
        }

        public static bool GetButtonUp(Buttons button) {
            return currentGamePadState.IsButtonUp(button) && !previousGamePadState.IsButtonUp(button);
        }

        public static bool GetButtonHeld(Buttons button) {
            return currentGamePadState.IsButtonDown(button);
        }

        public static bool GetAnyButton() {
            return GetButtonHeld(Buttons.A) || GetButtonHeld(Buttons.B) || GetButtonHeld(Buttons.X) || GetButtonHeld(Buttons.Y) || GetButtonHeld(Buttons.Back) || GetButtonHeld(Buttons.Start);
        }

        private static void UpdateGamepad() {
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);
        }
    }

    public enum MouseButtonState {
        None = 0,
        Left = 1,
        Right = 2,
        Both = 3
    }
}