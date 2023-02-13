using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Graphics {
    public class GraphicsController {
        private readonly GraphicsDeviceManager manager;
        private readonly Game game;

        private FullscreenMode _screenMode;
        private bool _useBorderless;
        private bool _verticalRetrace;
        private Resolution _resolution;

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

        public GraphicsController(Game _game, GraphicsDeviceManager _manager) {
            game = _game;
            manager = _manager;
        }

        private void ResolveResolution() {
            if (!IsFullScreen) _resolution = new Resolution(game.Window.ClientBounds);
            else _resolution = new Resolution(manager.GraphicsDevice.Adapter.CurrentDisplayMode.Width, manager.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
        }

        public void Apply() {
            manager.PreferredBackBufferWidth = _resolution.Width;
            manager.PreferredBackBufferHeight = _resolution.Height;
            manager.PreferredBackBufferFormat = SurfaceFormat.Color;
            manager.HardwareModeSwitch = (_screenMode == FullscreenMode.Borderless);
            manager.IsFullScreen = IsFullScreen;
            manager.SynchronizeWithVerticalRetrace = _verticalRetrace;
            manager.ApplyChanges();
        }

        public GraphicsController ToggleFullScreen() {
            _screenMode = IsFullScreen ? FullscreenMode.Windowed : (_useBorderless ? FullscreenMode.Borderless : FullscreenMode.Fullscreen);
            return this;            
        }
    }
}