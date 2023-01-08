using System;
using System.Diagnostics;
using Celestia.Screens;
using Celestia.UI;
using Celestia.GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Celestia.GUIs;
using System.Collections.Generic;

namespace Celestia
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static bool DebugMode { get; private set; }

        private double maximumFramerate = 280;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameWindow _window;

        private bool _isFullScreen = false;
        private bool _isBorderless = true;
        private int _windowedWidth = 0;
        private int _windowedHeight = 0;

        private List<GUI> globalGUIs;

        private IScreen _screen;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _window = Window;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Automatically enable debug mode when running a debug build.
            #if DEBUG
                DebugMode = true;
            #endif

            // Set up graphics and window (eventually from settings).
            SetupGraphicsAndWindow();

            // Set up the input manager.
            Input.Initialize();

            // Run XNA native initialization logic.
            base.Initialize();
        }

        private void SetupGraphicsAndWindow() {
            // Disable slowdown on window focus loss.
            InactiveSleepTime = new TimeSpan(0);

            // Set maximum framerate to avoid resource soaking.
            this.IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / maximumFramerate);

            // Allow game window to be resized, and set the title.
            Window.AllowUserResizing = true;
            Window.Title = "Celestia";

            // Make sure the UI knows what game window to refer to for screen space calculations.
            UIReferences.gameWindow = Window;
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
            _graphics.IsFullScreen = true;

            ApplyHardwareMode();
        }

        private void LeaveFullScreen() {
            _graphics.PreferredBackBufferWidth = _windowedWidth;
            _graphics.PreferredBackBufferHeight = _windowedHeight;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load global GUIs.
            LoadGUI();

            // Load the splash screen.
            LoadScreen(new SplashScreen(this));
        }

        private void LoadGUI() {
            globalGUIs = new List<GUI>();

            // Add Debug GUI.
            globalGUIs.Add(new DebugGUI());

            // Load each global GUI.
            globalGUIs.ForEach((gui) => { gui.Load(Content); });
        }

        public void LoadScreen(IScreen screen) {
            //Content.Unload();
            _screen?.Dispose();

            _screen = screen;
            _screen.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // Update the input.
            Input.Update();

            // Update the active screen.
            _screen.Update(gameTime);

            // Update each global GUI.
            globalGUIs.ForEach((gui) => { gui.Update(gameTime); });

            // If F3 is pressed, toggle Debug Mode.
            if (Input.Keyboard.GetKeyDown(Keys.F3)) DebugMode = !DebugMode;

            // If F11 is pressed, toggle Fullscreen.
            if (Input.Keyboard.GetKeyDown(Keys.F11)) ToggleFullScreen();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, _screen.GetSamplerState());

            // Draw the screen's content.
            _screen.Draw(_spriteBatch);

            // Draw each global GUI.
            globalGUIs.ForEach((gui) => { gui.Draw(_spriteBatch); });

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
