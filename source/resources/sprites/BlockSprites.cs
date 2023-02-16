using System.Collections.Generic;
using Celestia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celestia.Resources.Sprites {
    public class BlockFrames {
        private readonly int _size;
        private readonly float _scaling;

        private TextureRegion2D[] _frames;

        public BlockFrames(TextureAtlas atlas, int size, int startIndex, int frameCount) {
            _size = size;
            _scaling = 1f / _size;

            _frames = new TextureRegion2D[frameCount];
            for (int i = 0; i < frameCount; i++) 
                _frames[i] = atlas.GetRegion(startIndex + frameCount);
        }

        public void Draw(int index, SpriteBatch spriteBatch, Vector2 position, Color color) {
            spriteBatch.Draw(_frames[index % _frames.Length], position, color, 0f, Vector2.Zero, new Vector2(_scaling), SpriteEffects.None, 0f, null);
        }
    }
}