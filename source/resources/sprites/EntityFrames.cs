using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Sprites {
    public class EntityFrames {
        public SpriteEffects Effects;

        private readonly Vector2 _scaling;

        private TextureRegion2D[] _frames;

        public EntityFrames(TextureAtlas atlas, int startIndex, int frameCount, float scaling) {
            _scaling = new Vector2(scaling);

            _frames = new TextureRegion2D[frameCount];
            for (int i = 0; i < frameCount; i++) {
                _frames[i] = atlas.GetRegion(startIndex + i);
            }
        }

        private Vector2 GetOrigin(TextureRegion2D frame) {
            return new Vector2(0.5f * frame.Width, 0.5f * frame.Height);
        }

        public void Draw(int index, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, Color color) {
            spriteBatch.Draw(_frames[index % _frames.Length], position, color, 0f, GetOrigin(_frames[index % _frames.Length]), _scaling * scale, Effects, 0f, null);
        }
    }
}