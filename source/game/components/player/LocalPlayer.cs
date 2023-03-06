namespace Celesteia.Game.Components.Player {
    public class LocalPlayer {
        public float JumpRemaining = .5f;

        public void ResetJump(float attributeMax) {
            JumpRemaining = attributeMax;
        }
    }
}