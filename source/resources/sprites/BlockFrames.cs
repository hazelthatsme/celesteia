using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Sprites {
    public class BlockFrames {
        private readonly Vector2 _scaling;

        private BlockFrame[] _frames;

        private bool _doDraw;

        public BlockFrames(TextureAtlas atlas, int startIndex, int frameCount) {
            _doDraw = frameCount > 0;

            _scaling = new Vector2(ResourceManager.SPRITE_SCALING);

            _frames = new BlockFrame[frameCount];
            for (int i = 0; i < frameCount; i++) {
                _frames[i] = new BlockFrame(atlas.GetRegion(startIndex + i), _scaling);
            }
        }

        public BlockFrame GetFrame(int index) {
            if (!_doDraw) return null;
            
            return _frames[index % _frames.Length];
        }

        public BlockFrame GetProgressFrame(float progress) {
            return GetFrame((int)Math.Round(progress * (_frames.Length - 1)));
        }
    }

    public class BlockFrame {
        private TextureRegion2D _region;
        private Color[] _colors;
        private Texture2D _standaloneTexture;
        private Vector2 _scaling;
        

        public BlockFrame(TextureRegion2D region, Vector2 scaling) {
            _region = region;
            _scaling = scaling;

            CreateStandaloneTexture();
        }

        public void Draw(int index, SpriteBatch spriteBatch, Vector2 position, Color color, float depth = 0f) {
            spriteBatch.Draw(_standaloneTexture, position, _standaloneTexture.Bounds, color, 0f, Vector2.Zero, _scaling, SpriteEffects.None, depth);
        }

        private void CreateStandaloneTexture() {
            _colors = new Color[_region.Width * _region.Height];
            _region.Texture.GetData<Color>(0, _region.Bounds, _colors, 0, _colors.Length);

            _standaloneTexture = new Texture2D(_region.Texture.GraphicsDevice, _region.Width, _region.Height);
            _standaloneTexture.SetData<Color>(_colors);

            Debug.WriteLine($"  > Created texture.");
        }
    }
}