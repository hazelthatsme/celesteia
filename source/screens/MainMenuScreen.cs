using System.Diagnostics;
using Celestia.GUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using Microsoft.Xna.Framework.Media;
using Celestia.Graphics;
using MonoGame.Extended.Entities;
using Celestia.Screens.Systems.MainMenu;
using Celestia.Utilities.ECS;

namespace Celestia.Screens {
    public class MainMenuScreen : GameScreen
    {
        private new Game Game => (Game) base.Game;

        public MainMenuScreen(Game game) : base(game) {}

        private MainMenu mainMenu;

        private Song mainMenuTheme;

        private Camera2D Camera;
        private World _world;

        public override void LoadContent()
        {
            base.LoadContent();

            mainMenuTheme = Game.Content.Load<Song>("music/stargaze_symphony");
            Game.Music.PlayNow(mainMenuTheme);

            Camera = new Camera2D(GraphicsDevice);

            _world = new WorldBuilder()
                .AddSystem(new MainMenuBackgroundSystem())
                .AddSystem(new MainMenuRenderSystem(Camera, Game.SpriteBatch))
                .Build();

            new EntityFactory(_world, Game).CreateSkyboxPortion("stars", Color.White, -0.1f, .9f);
            //new EntityFactory(_world, Game).CreateSkyboxPortion("shadow", Color.White, 1f, 1f, .  8f);
            new EntityFactory(_world, Game).CreateSkyboxPortion("nebula", new Color(255,165,246,20), -2f, .3f);
            new EntityFactory(_world, Game).CreateSkyboxPortion("nebula", new Color(165,216,255,45), 3f, .5f);

            this.mainMenu = new MainMenu(Game);
            this.mainMenu.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _world.Draw(gameTime);
            this.mainMenu.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
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