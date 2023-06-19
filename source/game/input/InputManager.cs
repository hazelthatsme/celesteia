using Microsoft.Xna.Framework;

namespace Celesteia.Game.Input {
    public class InputManager : GameComponent {
        private new GameInstance Game => (GameInstance) base.Game;
        public InputManager(GameInstance Game) : base(Game) {}

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Enabled) return;

            KeyboardHelper.Update();
            GamepadHelper.Update();
            MouseHelper.Update();

            base.Update(gameTime);
        }

        public bool GetAny() {
            return KeyboardHelper.AnyKey() || GamepadHelper.AnyButton();
        }
    }
}