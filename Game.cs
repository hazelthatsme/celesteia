using System.Diagnostics;
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

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            instance = this;

            activeMenus = new Menu[16];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            activeMenus[0] = new PauseMenu();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // If either mouse button is clicked.
            if (((int) Mouse.GetState().LeftButton) + ((int) Mouse.GetState().RightButton) > 0) {
                activeMenus[0].ResolveMouseClick(Mouse.GetState().Position, Mouse.GetState().LeftButton, Mouse.GetState().RightButton);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            for (int index = 0; index < activeMenus.Length; index++)
                if (activeMenus[index]) activeMenus[index].Draw();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
