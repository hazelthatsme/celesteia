using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.Screens {
    public interface IScreen : IDisposable {
        void Load(ContentManager contentManager);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        SamplerState GetSamplerState();
    }
}