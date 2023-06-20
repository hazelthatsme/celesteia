using System.Diagnostics;
using Celesteia.GUIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using Microsoft.Xna.Framework.Media;
using Celesteia.Graphics;
using MonoGame.Extended.Entities;
using Celesteia.Game.Systems.MainMenu;
using Celesteia.Game.ECS;
using MonoGame.Extended;
using Celesteia.Game.Components.Skybox;
using Celesteia.Resources;

namespace Celesteia.Screens {
    public class MainMenuScreen : GameScreen
    {
        private new GameInstance Game => (GameInstance) base.Game;
        public MainMenuScreen(GameInstance game) : base(game) {}

        private MainMenu mainMenu;

        private Song mainMenuTheme;

        private Camera2D Camera;
        private World _world;

        public override void LoadContent()
        {
            base.LoadContent();

            mainMenuTheme = Content.Load<Song>("music/stargaze_symphony");
            Game.Music.PlayNow(mainMenuTheme);

            Camera = new Camera2D(GraphicsDevice);

            _world = new WorldBuilder()
                .AddSystem(new MainMenuBackgroundSystem())
                .AddSystem(new MainMenuRenderSystem(Camera, Game.SpriteBatch))
                .Build();

            CreateSkyboxPortion("stars", Color.White, -0.1f, .9f);
            CreateSkyboxPortion("shadow", Color.Black, 5f, .7f);
            CreateSkyboxPortion("shadow", Color.Black, 3f, .6f);
            CreateSkyboxPortion("nebula", new Color(165,216,255,45), 3f, .5f);
            CreateSkyboxPortion("nebula", new Color(255,165,246,45), -2f, .3f);

            mainMenu = new MainMenu(Game);
            mainMenu.LoadContent(Content);
        }

        public Entity CreateSkyboxPortion(string name, Color color, float rotation, float depth)
        {
            Entity e = _world.CreateEntity();

            e.Attach(new Transform2(Vector2.Zero, 0F, new Vector2(3f, 3f)));
            e.Attach(new SkyboxRotateZ(rotation));
            e.Attach(ResourceManager.Skybox.GetAsset(name).Frames.Clone().SetColor(color).SetDepth(depth));
            
            return e;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _world.Draw(gameTime);
            mainMenu.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            mainMenu.Update(gameTime, out _);
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