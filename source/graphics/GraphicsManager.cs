using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Graphics {
    public enum FullscreenMode {
        Windowed, Fullscreen, Borderless
    }
    
    public class GraphicsManager : GameComponent {
        private new GameInstance Game => (GameInstance) base.Game;
        private GraphicsDeviceManager _manager;
        public GraphicsManager(GameInstance Game) : base(Game) {
            _manager = new GraphicsDeviceManager(Game);
            _manager.PreferHalfPixelOffset = true;
        }

        private FullscreenMode _screenMode;
        private bool _useBorderless = false;
        private Rectangle _resolution = new Rectangle(0, 0, 1280, 720);
        private Rectangle _lastBounds;

        public FullscreenMode FullScreen {
            get { return _screenMode; }
            set { _screenMode = value; ResolveResolution(); }
        }

        public bool VSync = false;

        public bool IsFullScreen {
            get { return (_screenMode != FullscreenMode.Windowed); }
        }

        public Rectangle Resolution {
            get { return _resolution; }
            set { _lastBounds = _resolution = value; }
        }

        public bool MSAA = false;

        private void ResolveResolution() {
            if (!IsFullScreen) _resolution = _lastBounds;
            else {
                _lastBounds = Game.Window.ClientBounds;
                _resolution = new Rectangle(0, 0, _manager.GraphicsDevice.Adapter.CurrentDisplayMode.Width, _manager.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            }
        }

        public void Apply() {
            Game.Window.AllowUserResizing = true;
            _manager.PreferMultiSampling = MSAA;
            _manager.PreferredBackBufferWidth = _resolution.Width;
            _manager.PreferredBackBufferHeight = _resolution.Height;
            _manager.PreferredBackBufferFormat = SurfaceFormat.Color;
            _manager.HardwareModeSwitch = (_screenMode == FullscreenMode.Borderless);
            _manager.IsFullScreen = IsFullScreen;
            _manager.SynchronizeWithVerticalRetrace = VSync;
            _manager.ApplyChanges();
        }

        public GraphicsManager ToggleFullScreen() {
            FullScreen = IsFullScreen ? FullscreenMode.Windowed : (_useBorderless ? FullscreenMode.Borderless : FullscreenMode.Fullscreen);
            return this;
        }
    }
}