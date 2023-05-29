using System;
using Celesteia.Screens;
using Celesteia.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Celesteia.GUIs;
using System.Collections.Generic;
using Celesteia.Graphics;
using MonoGame.Extended.Screens;
using System.Linq;
using Celesteia.Resources;
using MonoGame.Extended.Screens.Transitions;
using Celesteia.Game.World;
using Celesteia.Resources.Collections;
using Celesteia.Game.Music;

namespace Celesteia
{
    public class GameInstance : Microsoft.Xna.Framework.Game
    {
        public static readonly string Version = "Alpha 1.3";
        public static bool DebugMode { get; private set; }
        
        private readonly List<string> cmdArgs;

        private List<GUI> globalGUIs;

        private readonly GraphicsManager _graphics;
        private readonly ScreenManager _screenManager;
        public readonly MusicManager Music;
        public readonly WorldManager Worlds;
        public readonly InputManager Input;

        public SpriteBatch SpriteBatch;

        public GameInstance()
        {
            // Add all components to the game instance.
            Components.Add(_graphics = new GraphicsManager(this));
            Components.Add(_screenManager = new ScreenManager());
            Components.Add(Music = new MusicManager(this));
            Components.Add(Worlds = new WorldManager(this));
            Components.Add(Input = new InputManager(this));

            // Load command line arguments into list.
            cmdArgs = Environment.GetCommandLineArgs().ToList();

            // Declare root of content management.
            Content.RootDirectory = "Content";

            // Make sure mouse is visible.
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

            // Run XNA native initialization logic.
            base.Initialize();
        }

        private void SetupGraphicsAndWindow() {
            _graphics.VSync = true;
            _graphics.FullScreen = FullscreenMode.Windowed;
            _graphics.Resolution = Window.ClientBounds;
            _graphics.Apply();
            
            // Disable slowdown on window focus loss.
            InactiveSleepTime = new TimeSpan(0);

            // Set maximum framerate to avoid resource soaking.
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 144.0);

            // Allow game window to be resized, and set the title.
            Window.AllowUserResizing = true;
            Window.Title = $"Celesteia {Version}";

            // Make sure the UI knows what game window to refer to for screen space calculations.
            UIReferences.gameWindow = Window;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.AddCollection(new BaseCollection(Content));
            ResourceManager.LoadContent(Content);

            // Load global GUIs.
            LoadGUI();

            // Load the splash screen if it's a release build, load the game directly if it's a debug build.
            if (cmdArgs.Contains("-gameplayDebug")) LoadScreen(new GameplayScreen(this, Worlds.LoadNewWorld((s) => Console.WriteLine(s)).GetAwaiter().GetResult()));
            else if (cmdArgs.Contains("-textDebug")) LoadScreen(new TextTestScreen(this));
            else LoadScreen(new SplashScreen(this));
        }

        private void LoadGUI() {
            globalGUIs = new List<GUI>();

            globalGUIs.Add(new DebugGUI(this));

            // Load each global GUI.
            globalGUIs.ForEach((gui) => { gui.LoadContent(Content); });
        }

        public void LoadScreen(GameScreen screen, Transition transition = null) {
            // If a transition is present, load the screen using it.
            if (transition != null) _screenManager.LoadScreen(screen, transition);

            // If there was no transition specified, load the screen without it.
            else _screenManager.LoadScreen(screen);
        }

        protected override void Update(GameTime gameTime)
        {
            // Update each global GUI.
            globalGUIs.ForEach((gui) => { gui.Update(gameTime, out _); });

            // If Scroll Lock is pressed, toggle GUIs.
            if (KeyboardHelper.Pressed(Keys.Scroll)) UIReferences.GUIEnabled = !UIReferences.GUIEnabled;

            // If F3 is pressed, toggle Debug Mode.
            if (KeyboardHelper.Pressed(Keys.F3)) DebugMode = !DebugMode;

            // If F11 is pressed, toggle Fullscreen.
            if (KeyboardHelper.Pressed(Keys.F11)) {
                _graphics.ToggleFullScreen();
                _graphics.Apply();
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
