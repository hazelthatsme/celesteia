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

        private Menu[] activeMenus;

        private IScreen _screen;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Allow game window to be resized.
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            instance = this;

            _screen = new SplashScreen(this);
            activeMenus = new Menu[16];

            Window.Title = "Celestia";

            _graphics.PreferredBackBufferWidth = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = _graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
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
