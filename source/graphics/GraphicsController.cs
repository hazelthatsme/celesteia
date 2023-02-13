using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Graphics {
    public class GraphicsController {
        private readonly GraphicsDeviceManager manager;
        private readonly Game game;

        private FullscreenMode _screenMode;
        private bool _useBorderless = true;
        private bool _verticalRetrace;
        private Rectangle _resolution;
        private Rectangle lastBounds;

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
            set { lastBounds = _resolution = value; }
        }

        public GraphicsController(Game _game, GraphicsDeviceManager _manager) {
            game = _game;
            manager = _manager;
        }

        private void ResolveResolution() {
            if (!IsFullScreen) _resolution = lastBounds;
            else {
                lastBounds = game.Window.ClientBounds;
                _resolution = new Rectangle(0, 0, manager.GraphicsDevice.Adapter.CurrentDisplayMode.Width, manager.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
            }
        }

        public void Apply() {
            game.Window.AllowUserResizing = true;
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