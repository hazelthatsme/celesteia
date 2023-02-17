using System;
using Celestia.Screens;
using Celestia.GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Celestia.GUIs;
using System.Collections.Generic;
using Celestia.Graphics;
using MonoGame.Extended.Screens;
using System.Linq;
using Celestia.Resources;
using MonoGame.Extended.Screens.Transitions;

namespace Celestia
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static bool DebugMode { get; private set; }
        
        private readonly List<string> cmdArgs;

        private double maximumFramerate = 144;

        private GraphicsDeviceManager _graphics;
        private GraphicsController GraphicsController;
        public SpriteBatch SpriteBatch;

        private List<GUI> globalGUIs;

        private readonly ScreenManager _screenManager;
        public readonly MusicManager Music;

        public Game()
        {
            // Graphics setup.
            _graphics = new GraphicsDeviceManager(this);
            GraphicsController = new GraphicsController(this, _graphics);

            // Load command line arguments into list.
            cmdArgs = Environment.GetCommandLineArgs().ToList();

            // Declare root of content management.
            Content.RootDirectory = "Content";

            // Make sure mouse is visible.
            IsMouseVisible = true;
            
            // Load the screen manager.
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);

            Music = new MusicManager(this);
            Components.Add(Music);
        }

        protected override void Initialize()
        {
            // Automatically enable debug mode when running a debug build.
            #if DEBUG
                DebugMode = true;
            #endif

            // Set up graphics and window (eventually from settings).
            SetupGraphicsAndWindow();

            // Initialize input management.
            Input.Initialize();

            // Run XNA native initialization logic.
            base.Initialize();
        }

        private void SetupGraphicsAndWindow() {
            GraphicsController.VSync = true;
            GraphicsController.FullScreen = FullscreenMode.Windowed;
            GraphicsController.Resolution = Window.ClientBounds;
            GraphicsController.Apply();
            
            // Disable slowdown on window focus loss.
            InactiveSleepTime = new TimeSpan(0);

            // Set maximum framerate to avoid resource soaking.
            IsFixedTimeStep = true;
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

            ResourceManager.LoadContent(Content);

            // Load global GUIs.
            LoadGUI();

            // Load the splash screen if it's a release build, load the game directly if it's a debug build.
            if (cmdArgs.Contains("-gameplayDebug")) LoadScreen(new GameplayScreen(this));
            else LoadScreen(new SplashScreen(this));
        }

        private void LoadGUI() {
            globalGUIs = new List<GUI>();

            globalGUIs.Add(new DebugGUI(this));

            // Load each global GUI.
            globalGUIs.ForEach((gui) => { gui.LoadContent(); });
        }

        public void LoadScreen(GameScreen screen, Transition transition) {
            _screenManager.LoadScreen(screen, transition);
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
            if (KeyboardWrapper.GetKeyDown(Keys.F11)) {
                GraphicsController.ToggleFullScreen();
                GraphicsController.Apply();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            globalGUIs.ForEach((gui) => { gui.Draw(gameTime); });
        }
    }
}
