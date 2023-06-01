using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Sprites {
    public class BlockFrames {
        private readonly Vector2 _scaling;

        private BlockFrame[] _frames;

        private bool _doDraw;
        public bool DoDraw => _doDraw;

        public BlockFrames(TextureAtlas atlas, int startIndex, int frameCount, Vector2? origin = null) {
            _doDraw = frameCount > 0;

            _scaling = new Vector2(ResourceManager.SPRITE_SCALING);

            _frames = new BlockFrame[frameCount];
            for (int i = 0; i < frameCount; i++) {
                _frames[i] = new BlockFrame(atlas.GetRegion(startIndex + i), _scaling, origin);
            }
        }

        public BlockFrame GetFrame(int index) {
            if (!_doDraw) return null;
            
            return _frames[index % _frames.Length];
        }

        public BlockFrame GetProgressFrame(float progress) {
            return GetFrame((int)MathF.Round(progress * (_frames.Length - 1)));
        }
    }

    public class BlockFrame {
        private TextureRegion2D _region;
        private Vector2 _scaling;
        private Vector2 _origin;

        public BlockFrame(TextureRegion2D region, Vector2 scaling, Vector2? origin = null) {
            _region = region;
            _scaling = scaling;
            _origin = origin.HasValue ? origin.Value : Vector2.Zero;
        }

        public void Draw(int index, SpriteBatch spriteBatch, Vector2 position, Color color, float depth = 0f)
        => spriteBatch.Draw(_region, position, color, 0f, _origin, _scaling, SpriteEffects.None, depth);
        
        public TextureRegion2D GetRegion() => _region;
    }
}