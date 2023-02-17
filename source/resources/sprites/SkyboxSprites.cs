using System.Collections.Generic;
using Celestia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System.Diagnostics;

namespace Celestia.Resources.Sprites {
    public class SkyboxPortionFrames {
        private readonly Vector2 _scaling;

        private TextureRegion2D[] _frames;
        private Color _color;
        private float _depth;
        private float _alpha;

        public SkyboxPortionFrames(TextureAtlas atlas, int size, int startIndex, int frameCount) {
            _scaling = new Vector2(ResourceManager.SPRITE_SCALING);

            _frames = new TextureRegion2D[frameCount];
            for (int i = 0; i < frameCount; i++) {
                _frames[i] = atlas.GetRegion(startIndex + i);
            }
        }

        public SkyboxPortionFrames SetColor(Color color) {
            _color = color;
            return this;
        }

        public SkyboxPortionFrames SetAlpha(float alpha) {
            _alpha = alpha;
            return this;
        }

        public SkyboxPortionFrames SetDepth(float depth) {
            _depth = depth;
            return this;
        }

        private Vector2 GetOrigin(TextureRegion2D frame) {
            return new Vector2(0.5f * frame.Width, 0.5f * frame.Height);
        }

        public void Draw(int index, SpriteBatch spriteBatch, Vector2 position, float rotation, Vector2 scale) {
            spriteBatch.Draw(_frames[index % _frames.Length], position, _color * _alpha, rotation, GetOrigin(_frames[index % _frames.Length]), scale * _scaling, SpriteEffects.None, _depth, null);
        }
    }
}