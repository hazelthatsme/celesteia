using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celesteia.GameInput {
    public class GamepadWrapper {
        private PlayerIndex index = PlayerIndex.One;

        private GamePadState currentState;
        private GamePadState previousState;

        public void Update() {
            UpdatePlayerIndex();
            UpdateState();
        }

        private void UpdatePlayerIndex() {
            GamePadState state;
            for (int i = 0; i < 4; i++) {
                state = GamePad.GetState((PlayerIndex) i);
                if (state.IsConnected) {
                    index = (PlayerIndex) i;
                    break;
                }
            }
        }

        private void UpdateState() {
            previousState = currentState;
            currentState = GamePad.GetState(index);
        }

        public bool GetAnyButton() {
            return GetAnyButton(currentState);
        }

        public bool GetButtonDown(Buttons buttons) {
            return GetButtonHeld(currentState, buttons) && !GetButtonHeld(previousState, buttons);
        }

        public bool GetButtonUp(Buttons buttons) {
            return !GetButtonHeld(currentState, buttons) && GetButtonHeld(previousState, buttons);
        }

        private static bool GetAnyButton(GamePadState state) {
            return  GetButtonHeld(state, Buttons.A) || 
                    GetButtonHeld(state, Buttons.B) || 
                    GetButtonHeld(state, Buttons.X) || 
                    GetButtonHeld(state, Buttons.Y) || 
                    GetButtonHeld(state, Buttons.Start) || 
                    GetButtonHeld(state, Buttons.Back);
        }

        private static bool GetButtonHeld(GamePadState state, Buttons buttons) {
            return state.IsButtonDown(buttons);
        }
    }
}