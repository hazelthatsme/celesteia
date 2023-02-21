using System.Diagnostics;
using Celesteia.Screens.Systems;
using Microsoft.Xna.Framework;
using Celesteia.Game.ECS;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using Celesteia.Resources;
using Celesteia.Graphics;
using Celesteia.Game.Worlds;
using Celesteia.Game.Worlds.Generators;

namespace Celesteia.Screens {
    public class GameplayScreen : GameScreen {
        private new GameInstance Game => (GameInstance) base.Game;

        public GameplayScreen(GameInstance game) : base(game) {}

        private Camera2D Camera;
        private World _world;
        private EntityFactory _entityFactory;
        private GameWorld _gameWorld;

        public override void LoadContent()
        {
            base.LoadContent();

            Game.Music.PlayNow(null);

            Camera = new Camera2D(GraphicsDevice);

            _gameWorld = new GameWorld(2, 1);
            _gameWorld.SetGenerator(new TerranWorldGenerator(_gameWorld));
            _gameWorld.Generate();

            _world = new WorldBuilder()
                .AddSystem(new GameWorldSystem(Camera, Game.SpriteBatch, _gameWorld))
                .AddSystem(new LocalPlayerSystem())
                .AddSystem(new CameraFollowSystem(Camera))
                .AddSystem(new CameraRenderSystem(Camera, Game.SpriteBatch))
                .AddSystem(new CameraZoomSystem(Camera))
                .Build();
                
            _entityFactory = new EntityFactory(_world, Game);
            
            ResourceManager.Entities.Types.Find(x => x.EntityID == 0).Instantiate(_world);
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.SkyBlue);
            _world.Draw(gameTime);
        }

        public override void Dispose()
        {
            Debug.WriteLine("Unloading GameplayScreen content...");
            base.UnloadContent();
            Debug.WriteLine("Disposing GameplayScreen...");
            base.Dispose();
        }
    }
}