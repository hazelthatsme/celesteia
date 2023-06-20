using System.Diagnostics;
using Celesteia.Game.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Celesteia.Game.ECS;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Screens;
using Celesteia.Resources;
using Celesteia.Graphics;
using Celesteia.Game.Components;
using Celesteia.Game.Systems.Physics;
using Celesteia.GUIs.Game;
using Celesteia.Game.Systems.UI;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Planets;
using Microsoft.Xna.Framework.Media;

namespace Celesteia.Screens {
    public class GameplayScreen : GameScreen {
        private new GameInstance Game => (GameInstance) base.Game;

        private GameWorld _gameWorld;
        private ChunkMap _chunkMap;
        public GameplayScreen(GameInstance game, GameWorld gameWorld) : base(game) {
            _gameWorld = gameWorld;
            _chunkMap = gameWorld.ChunkMap;
        }

        private SpriteBatch SpriteBatch => Game.SpriteBatch;
        private Camera2D Camera;
        private GameGUI _gameGui;

        public override void LoadContent()
        {
            base.LoadContent();

            Song overworldMusic = Content.Load<Song>("music/landing_light");
            Game.Music.PlayNow(overworldMusic);

            Camera = new Camera2D(GraphicsDevice);

            _gameGui = new GameGUI(Game);
            _gameGui.LoadContent(Content);

            LocalPlayerSystem lps = new LocalPlayerSystem(Game, _chunkMap, Camera, SpriteBatch, _gameGui);

            _gameWorld.BeginBuilder()
                .AddSystem(new PhysicsSystem())
                .AddSystem(new PhysicsWorldCollisionSystem(_chunkMap))
                .AddSystem(new TargetPositionSystem(_chunkMap))
                .AddSystem(new ChunkMapRenderSystem(Camera, SpriteBatch, _chunkMap))
                .AddSystem(new CameraSystem(Camera))
                .AddSystem(new CameraRenderSystem(Camera, SpriteBatch))
                .AddSystem(new LightingSystem(Camera, SpriteBatch, _chunkMap))
                .AddSystem(lps)
                .AddSystem(new GameGUIDrawSystem(_gameGui));
            _gameWorld.EndBuilder();

            Entity player = new EntityFactory(_gameWorld).CreateEntity(NamespacedKey.Base("player"));
            player.Get<TargetPosition>().Target = _chunkMap.GetSpawnpoint();
            _gameGui.SetReferenceInventory(player.Get<Inventory>());
            lps.Player = player;
        }

        public override void Update(GameTime gameTime) => _gameWorld.Update(gameTime);

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);
            _gameWorld.Draw(gameTime);
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