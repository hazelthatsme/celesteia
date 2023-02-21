using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.Graphics {
    public class GraphicsController {
        private readonly GraphicsDeviceManager manager;
        private readonly GameInstance game;

        private FullscreenMode _screenMode;
        private bool _useBorderless = false;
        private bool _verticalRetrace;
        private Rectangle _resolution = new Rectangle(0, 0, 1280, 720);
        private Rectangle _lastBounds;
        private bool _multiSampling = false;

        public FullscreenMode FullScreen {
            get { return _screenMode; }
            set { _screenMode = value; ResolveResolution(); }
        }

        public bool VSync {
            get { return _verticalRetrace; }
            set { _verticalRetrace = value; }
        }

        public bool IsFullScreen {
            get { return (_screenMode != FullscreenMode.Windowed); }
        }

        public Rectangle Resolution {
            get { return _resolution; }
            set { _lastBounds = _resolution = value; }
        }

        public bool MSAA {
            get { return _multiSampling; }
            set { _multiSampling = value; }
        }

        public GraphicsController(GameInstance _game, GraphicsDeviceManager _manager) {
            game = _game;
            manager = _manager;
        }

        private void ResolveResolution() {
            if (!IsFullScreen) _resolution = _lastBounds;
            else {
                _lastBounds = game.Window.ClientBounds;
                _resolution = new Rectangle(0, 0, manager.GraphicsDevice.Adapter.CurrentDisplayMode.Width, manager.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            }
        }

        public void Apply() {
            game.Window.AllowUserResizing = true;
            manager.PreferMultiSampling = _multiSampling;
            manager.PreferredBackBufferWidth = _resolution.Width;
            manager.PreferredBackBufferHeight = _resolution.Height;
            manager.PreferredBackBufferFormat = SurfaceFormat.Color;
            manager.HardwareModeSwitch = (_screenMode == FullscreenMode.Borderless);
            manager.IsFullScreen = IsFullScreen;
            manager.SynchronizeWithVerticalRetrace = _verticalRetrace;
            manager.ApplyChanges();
        }

        public GraphicsController ToggleFullScreen() {
            FullScreen = IsFullScreen ? FullscreenMode.Windowed : (_useBorderless ? FullscreenMode.Borderless : FullscreenMode.Fullscreen);
            return this;
        }
    }
}