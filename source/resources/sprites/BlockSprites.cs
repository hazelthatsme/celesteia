using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Resources.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Sprites {
    public class BlockFrames {
        private readonly int _size;
        private readonly Vector2 _scaling;

        private BlockFrame[] _frames;

        private bool _doDraw;

        public BlockFrames(TextureAtlas atlas, int size, int startIndex, int frameCount) {
            _doDraw = frameCount > 0;

            _size = size;
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

        public void Draw(int index, SpriteBatch spriteBatch, Vector2 position, Color color) {
            spriteBatch.Draw(GetTexture(), position, GetTexture().Bounds, color, 0f, Vector2.Zero, _scaling, SpriteEffects.None, 0f);
        }

        public Texture2D GetTexture() {
            return _standaloneTexture;
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