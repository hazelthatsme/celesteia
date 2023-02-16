using System.Diagnostics;
using Celestia.GameInput;
using Celestia.UI;
using Celestia.GUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using Microsoft.Xna.Framework.Media;

namespace Celestia.Screens {
    public class MainMenuScreen : GameScreen
    {
        private new Game Game => (Game) base.Game;

        private MainMenu mainMenu;

        private Song mainMenuTheme;

        public MainMenuScreen(Game game) : base(game) {}

        public override void LoadContent()
        {
            base.LoadContent();

            mainMenuTheme = Game.Content.Load<Song>("music/stargaze_symphony");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(mainMenuTheme);

            this.mainMenu = new MainMenu(Game);
            this.mainMenu.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            this.mainMenu.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.mainMenu.Update(gameTime);
        }

        public override void Dispose()
        {
            Debug.WriteLine("Unloading MainMenuScreen content...");
            base.UnloadContent();
            Debug.WriteLine("Disposing MainMenuScreen...");
            base.Dispose();
        }
    }
}