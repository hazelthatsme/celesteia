using System.Diagnostics;
using Celesteia.Game.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Game.ECS;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using Celesteia.Resources;
using Celesteia.Graphics;
using Celesteia.Game.Worlds;
using Celesteia.Game.Components;
using Celesteia.Game.Systems.Physics;
using Celesteia.GUIs.Game;
using Celesteia.Game.Systems.UI;
using Celesteia.Resources.Collections;

namespace Celesteia.Screens {
    public class GameplayScreen : GameScreen {
        private new GameInstance Game => (GameInstance) base.Game;

        public GameplayScreen(GameInstance game, GameWorld gameWorld) : base(game) {
            _gameWorld = gameWorld;
        }

        private SpriteBatch SpriteBatch => Game.SpriteBatch;
        private Camera2D Camera;
        private World _world;
        private EntityFactory _entityFactory;
        private GameWorld _gameWorld;
        private GameGUI _gameGui;

        public override void LoadContent()
        {
            base.LoadContent();

            Game.Music.PlayNow(null);

            Camera = new Camera2D(GraphicsDevice);

            _gameGui = new GameGUI(Game);
            _gameGui.LoadContent(Content);

            LocalPlayerSystem localPlayerSystem;

            _world = new WorldBuilder()
                .AddSystem(new PhysicsGravitySystem(_gameWorld))
                .AddSystem(new PhysicsSystem())
                .AddSystem(new PhysicsWorldCollisionSystem(_gameWorld))
                .AddSystem(localPlayerSystem = new LocalPlayerSystem(_gameGui, Camera, _gameWorld))
                .AddSystem(new TargetPositionSystem(_gameWorld))
                .AddSystem(new CameraFollowSystem(Camera))
                .AddSystem(new CameraZoomSystem(Camera))
                .AddSystem(new GameWorldRenderSystem(Camera, SpriteBatch, _gameWorld))
                .AddSystem(new CameraRenderSystem(Camera, SpriteBatch))
                .AddSystem(new LightingSystem(Camera, SpriteBatch, _gameWorld))
                .AddSystem(new GameGUIDrawSystem(_gameGui))
                //.AddSystem(new PhysicsCollisionDebugSystem(Camera, SpriteBatch, _gameWorld))
                //.AddSystem(new EntityDebugSystem(Camera, SpriteBatch))
                .Build();

            _entityFactory = new EntityFactory(_world, Game);

            Entity player = _entityFactory.CreateEntity(ResourceManager.Entities.GetResource(NamespacedKey.Base("player")) as EntityType);
            player.Get<TargetPosition>().Target = _gameWorld.GetSpawnpoint();
            _gameGui.SetReferenceInventory(player.Get<EntityInventory>().Inventory);
            localPlayerSystem.Player = player;
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