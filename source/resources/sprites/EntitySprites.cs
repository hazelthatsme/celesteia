using System.Collections.Generic;
using System.Diagnostics;
using Celestia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celestia.Resources.Sprites {
    public class EntityFrames {
        private readonly float _scaling;

        private TextureRegion2D[] _frames;

        public EntityFrames(TextureAtlas atlas, int startIndex, int frameCount, float scaling) {
            _scaling = scaling;

            _frames = new TextureRegion2D[frameCount];
            for (int i = 0; i < frameCount; i++) {
                Debug.WriteLine(startIndex + i);
                _frames[i] = atlas.GetRegion(startIndex + i);
            }
                
        }

        public void Draw(int index, SpriteBatch spriteBatch, Vector2 position, Color color) {
            Debug.WriteLine(_scaling);
            spriteBatch.Draw(_frames[index % _frames.Length], position, color, 0f, Vector2.Zero, new Vector2(_scaling), SpriteEffects.None, 0f, null);
        }
    }
}