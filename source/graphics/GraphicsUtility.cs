using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Graphics {
    public static class GraphicsUtility {
        private static bool _isFullScreen;
        private static bool _isBorderless;
        private static int _windowedWidth = 0;
        private static int _windowedHeight = 0;

        public static void ToggleFullScreen(GameWindow _window, GraphicsDeviceManager _graphics) {
            _isFullScreen = !_isFullScreen;
            ApplyFullscreenChange(_window, _graphics);
        }

        public static void ApplyFullscreenChange(GameWindow _window, GraphicsDeviceManager _graphics) {
            if (_isFullScreen) GoFullScreen(_window, _graphics);
            else LeaveFullScreen(_window, _graphics);
        }

        public static void ApplyHardwareMode(GameWindow _window, GraphicsDeviceManager _graphics) {
            _graphics.HardwareModeSwitch = !_isBorderless;
            _graphics.ApplyChanges();
        }

        private static void GoFullScreen(GameWindow _window, GraphicsDeviceManager _graphics) {
            _windowedWidth = _window.ClientBounds.Width;
            _windowedHeight = _window.ClientBounds.Height;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;

            ApplyHardwareMode(_window, _graphics);
        }

        private static void LeaveFullScreen(GameWindow _window, GraphicsDeviceManager _graphics) {
            _graphics.PreferredBackBufferWidth = _windowedWidth;
            _graphics.PreferredBackBufferHeight = _windowedHeight;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }
    }
}