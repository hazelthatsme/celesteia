using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Sprites {
    public class SkyboxPortionFrames {
        private readonly Vector2 _scaling;

        private TextureAtlas _atlas;
        private int _start;
        private int _count;

        private TextureRegion2D[] _frames;
        private Color _color;
        private float _depth;

        public SkyboxPortionFrames(TextureAtlas atlas, int size, int startIndex, int frameCount) {
            _atlas = atlas;
            _start = startIndex;
            _count = frameCount;
            _scaling = new Vector2(ResourceManager.SPRITE_SCALING) / 2f;

            _frames = new TextureRegion2D[frameCount];
            for (int i = 0; i < frameCount; i++) {
                _frames[i] = atlas.GetRegion(startIndex + i);
            }
        }

        public SkyboxPortionFrames SetColor(Color color) {
            _color = color;
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
            spriteBatch.Draw(_frames[index % _frames.Length], position, _color, rotation, GetOrigin(_frames[index % _frames.Length]), scale * _scaling, SpriteEffects.None, _depth, null);
        }
        
        public SkyboxPortionFrames Clone() {
            return new SkyboxPortionFrames(_atlas, 0, _start, _count).SetColor(_color).SetDepth(_depth);
        }
    }
}