using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;

namespace Celestia.Screens {
    public class GameplayScreen : GameScreen {
        private new Game Game => (Game) base.Game;

        public GameplayScreen(Game game) : base(game) {}

        private OrthographicCamera Camera;

        float offset;

        public override void LoadContent()
        {
            base.LoadContent();

            BoxingViewportAdapter viewportAdapter = new BoxingViewportAdapter(Game.Window, Game.GraphicsDevice, 800, 450);
            Camera = new OrthographicCamera(viewportAdapter);
        }

        public override void Update(GameTime gameTime)
        {
            offset += gameTime.ElapsedGameTime.Milliseconds / 20f;

            Camera.LookAt(new Vector2(offset * 2, offset * 2));
        }

        public override void Draw(GameTime gameTime)
        {
            Matrix viewMatrix = Camera.GetViewMatrix();
            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, null, null, null, viewMatrix);
            Game.SpriteBatch.FillRectangle(new RectangleF(offset, offset, 50, 50), Color.Wheat, 1f);
            Game.SpriteBatch.End();
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