using System;
using System.Diagnostics;
using Celestia.Screens;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Celestia
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        /* BEGINNING OF EXPERIMENTAL INSTANCE-BASED CONTROLS. */
        /* Maybe do this a little more neatly? */
        private static Game instance;

        public static GameWindow GetGameWindow() { return instance.Window; }

        public static SpriteBatch GetSpriteBatch() { return instance._spriteBatch; }

        /* END OF EXPERIMENTAL STUFF. */

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameWindow _window;

        private bool _isFullScreen = false;
        private bool _isBorderless = true;
        private int _windowedWidth = 0;
        private int _windowedHeight = 0;

        private Menu[] activeMenus;

        private IScreen _screen;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _window = Window;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Allow game window to be resized.
            Window.AllowUserResizing = true;
        }

        public void ToggleFullScreen() {
            _isFullScreen = !_isFullScreen;
            ApplyFullscreenChange();
        }

        public void ApplyFullscreenChange() {
            if (_isFullScreen) GoFullScreen();
            else LeaveFullScreen();
        }

        private void ApplyHardwareMode() {
            _graphics.HardwareModeSwitch = !_isBorderless;
            _graphics.ApplyChanges();
        }

        private void GoFullScreen() {
            _windowedWidth = Window.ClientBounds.Width;
            _windowedHeight = Window.ClientBounds.Height;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Debug.WriteLine(_graphics.PreferredBackBufferWidth);
            Debug.WriteLine(_graphics.PreferredBackBufferHeight);
            _graphics.IsFullScreen = true;

            ApplyHardwareMode();
        }

        private void LeaveFullScreen() {
            _graphics.PreferredBackBufferWidth = _windowedWidth;
            _graphics.PreferredBackBufferHeight = _windowedHeight;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            instance = this;

            _screen = new SplashScreen(this);
            activeMenus = new Menu[16];

            Window.Title = "Celestia";

            //_graphics.PreferMultiSampling = false;
            GoFullScreen();
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the splash screen.
            LoadScreen(new SplashScreen(this));
        }

        public void LoadScreen(IScreen screen) {
            _screen?.Dispose();

            _screen = screen;
            _screen.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (Input.GetKeyDown(Keys.F11)) ToggleFullScreen();

            _screen.Update((float) (gameTime.ElapsedGameTime.TotalMilliseconds / 1000f));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw the screen's content.
            _screen.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
