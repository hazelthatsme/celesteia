using Microsoft.Xna.Framework;

namespace Celesteia.Game.Input {
    public class InputManager : GameComponent {
        public static InputManager Instance { get; private set; }

        private new GameInstance Game => (GameInstance) base.Game;
        public InputManager(GameInstance Game) : base(Game) {}

        public KeyboardWrapper Keyboard { get; private set; }
        public MouseWrapper Mouse { get; private set; }
        public GamepadWrapper Gamepad { get; private set; }

        public override void Initialize()
        {
            Instance = this;

            Keyboard = new KeyboardWrapper();
            Mouse = new MouseWrapper();
            Gamepad = new GamepadWrapper();

            base.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            Instance = null;
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {
            Keyboard.Update();
            Mouse.Update();
            Gamepad.Update();

            base.Update(gameTime);
        }

        public bool GetAny() {
            return Keyboard.GetAnyKey() || Gamepad.GetAnyButton();
        }
    }
}