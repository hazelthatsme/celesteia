using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Screens {
    public class GameScreen : IScreen {
        private Game gameRef;

        public GameScreen(Game gameRef) {
            this.gameRef = gameRef;
        }

        public void Load(ContentManager contentManager)
        {
        }

        public void Update(float deltaTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public SamplerState GetSamplerState()
        {
            return SamplerState.PointClamp;
        }

        public void Dispose()
        {
            Debug.WriteLine("Disposing GameScreen...");
        }
    }
}