namespace Celesteia.Game.Components.Player {
    public class LocalPlayer {
        public float MaxJumpDuration = .5f;
        public float JumpRemaining = .5f;

        public void ResetJump() {
            JumpRemaining = MaxJumpDuration;
        }
    }
}