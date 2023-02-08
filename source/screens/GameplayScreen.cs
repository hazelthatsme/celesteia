using System;
using System.Diagnostics;
using Celestia.Screens.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Celestia.Utilities.ECS;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Sprites;

namespace Celestia.Screens {
    public class GameplayScreen : GameScreen {
        private new Game Game => (Game) base.Game;

        public GameplayScreen(Game game) : base(game) {}

        private OrthographicCamera Camera;
        private World _world;
        private EntityFactory _entityFactory;

        public override void LoadContent()
        {
            base.LoadContent();

            BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Game.Window, Game.GraphicsDevice, 800, 450);
            Camera = new OrthographicCamera(viewportAdapter);
            Camera.Zoom = 5f;

            _world = new WorldBuilder()
                .AddSystem(new LocalPlayerSystem())
                .AddSystem(new CameraFollowSystem(Camera))
                .AddSystem(new CameraRenderSystem(Camera, Game.SpriteBatch))
                .Build();
                
            _entityFactory = new EntityFactory(_world, Game);
            
            _entityFactory.CreatePlayer();
        }

        public override void Update(GameTime gameTime)
        {
            Camera.LookAt(Vector2.Zero);
            _world.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
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