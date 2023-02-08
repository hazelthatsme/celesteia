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
using Celestia.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Input.InputListeners;

namespace Celestia
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static bool DebugMode { get; private set; }

        private double maximumFramerate = 144;

        private GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch;

        private List<GUI> globalGUIs;

        private readonly ScreenManager _screenManager;

        public KeyboardListener _keyboard;
        public MouseListener _mouse;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // Load the screen manager.
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
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
            _keyboard = new KeyboardListener();
            _mouse = new MouseListener();

            Input.Initialize();
            Components.Add(new InputListenerComponent(this, _keyboard, _mouse));

            // Run XNA native initialization logic.
            base.Initialize();
        }

        private void SetupGraphicsAndWindow() {
            // Disable slowdown on window focus loss.
            InactiveSleepTime = new TimeSpan(0);

            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            // Set maximum framerate to avoid resource soaking.
            this.IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / maximumFramerate);

            // Allow game window to be resized, and set the title.
            Window.AllowUserResizing = true;
            Window.Title = "Celestia";

            // Make sure the UI knows what game window to refer to for screen space calculations.
            UIReferences.gameWindow = Window;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load global GUIs.
            LoadGUI();

            // Load the splash screen if it's a release build, load the game directly if it's a debug build.
            #if DEBUG
                LoadScreen(new GameplayScreen(this));
            #else
                LoadScreen(new SplashScreen(this));
            #endif
        }

        private void LoadGUI() {
            globalGUIs = new List<GUI>();

            globalGUIs.Add(new DebugGUI(this));

            // Load each global GUI.
            globalGUIs.ForEach((gui) => { gui.LoadContent(); });
        }

        public void LoadScreen(GameScreen screen) {
            _screenManager.LoadScreen(screen);
        }

        protected override void Update(GameTime gameTime)
        {
            // Update the input.
            Input.Update();

            // Update each global GUI.
            globalGUIs.ForEach((gui) => { gui.Update(gameTime); });

            // If F3 is pressed, toggle Debug Mode.
            if (KeyboardWrapper.GetKeyDown(Keys.F3)) DebugMode = !DebugMode;

            // If F11 is pressed, toggle Fullscreen.
            if (KeyboardWrapper.GetKeyDown(Keys.F11)) GraphicsUtility.ToggleFullScreen(Window, _graphics);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            globalGUIs.ForEach((gui) => { gui.Draw(gameTime); });
        }
    }
}
