using System;
using Celesteia.Resources;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI {
    public class Button : IClickableElement {
        private Rect _rect = Rect.AbsoluteZero;

        private ButtonColorGroup _colorGroup = new ButtonColorGroup(Color.White);

        public delegate void ClickEvent(Point position);
        private ClickEvent _onClick = null;

        private bool _isEnabled = true;

        private bool _mouseOver;

        private Texture2D _texture;

        private string _text = "";
        private TextAlignment _textAlignment = TextAlignment.Left;
        private FontType _font;
        private float _fontSize;

        private TextureAtlas _patches;
        private int _patchSize;

        public Button(Rect rect) {
            _rect = rect;
        }

        public Button SetOnClick(ClickEvent onClick) {
            _onClick = onClick;
            return this;
        }

        public Button SetTexture(Texture2D texture) {
            _texture = texture;
            return this;
        }

        public Button MakePatches(int size) {
            if (_texture != null) {
                _patchSize = size;
                _patches = TextureAtlas.Create("buttonPatches", _texture, _patchSize, _patchSize);
            }
            return this;
        }

        public Button SetFont(FontType font) {
            _font = font;
            return this;
        }

        public Button SetText(string text) {
            _text = text;
            return this;
        }
        
        public Button SetFontSize(float fontSize) {
            _fontSize = fontSize;
            return this;
        }

        public Button SetTextAlignment(TextAlignment textAlignment) {
            _textAlignment = textAlignment;
            return this;
        }

        public Button SetColorGroup(ButtonColorGroup colorGroup) {
            _colorGroup = colorGroup;
            return this;
        }

        public void OnClick(Point position) {
            _onClick?.Invoke(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button's texture.
            if (_patches != null) DrawPatched(spriteBatch, _rect.ToXnaRectangle());
            else spriteBatch.Draw(GetTexture(spriteBatch), _rect.ToXnaRectangle(), null, GetColor());

            TextUtilities.DrawAlignedText(spriteBatch, _font, _text, _textAlignment, _rect, 24f);
        }

        private int _scaledPatchSize => _patchSize * UIReferences.Scaling;
        private void DrawPatched(SpriteBatch spriteBatch, Rectangle r) {
            int y;

            // Top
            y = r.Y;
            {
                // Top left
                spriteBatch.Draw(_patches.GetRegion(0), new Rectangle(r.X, y, _scaledPatchSize, _scaledPatchSize), GetColor());

                // Top center
                spriteBatch.Draw(_patches.GetRegion(1), new Rectangle(r.X + _scaledPatchSize, y, r.Width - (2 * _scaledPatchSize), _scaledPatchSize), GetColor());

                // Top right
                spriteBatch.Draw(_patches.GetRegion(2), new Rectangle(r.X + r.Width - _scaledPatchSize, y, _scaledPatchSize, _scaledPatchSize), GetColor());
            }

            // Center
            y = r.Y + _scaledPatchSize;
            {
                // Left center
                spriteBatch.Draw(_patches.GetRegion(3), new Rectangle(r.X, y, _scaledPatchSize, r.Height - (2 * _scaledPatchSize)), GetColor());

                // Center
                spriteBatch.Draw(_patches.GetRegion(4), new Rectangle(r.X + _scaledPatchSize, y, r.Width - (2 * _scaledPatchSize), r.Height - (2 * _scaledPatchSize)), GetColor());

                // Right center
                spriteBatch.Draw(_patches.GetRegion(5), new Rectangle(r.X + r.Width - _scaledPatchSize, y, _scaledPatchSize, r.Height - (2 * _scaledPatchSize)), GetColor());
            }

            // Bottom
            y = r.Y + r.Height - _scaledPatchSize;
            {
                // Bottom left
                spriteBatch.Draw(_patches.GetRegion(6), new Rectangle(r.X, y, _scaledPatchSize, _scaledPatchSize), GetColor());

                // Bottom center
                spriteBatch.Draw(_patches.GetRegion(7), new Rectangle(r.X + _scaledPatchSize, y, r.Width - (2 * _scaledPatchSize), _scaledPatchSize), GetColor());

                // Bottom right
                spriteBatch.Draw(_patches.GetRegion(8), new Rectangle(r.X + r.Width - _scaledPatchSize, y, _scaledPatchSize, _scaledPatchSize), GetColor());
            }
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            if (_texture == null) {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _texture.SetData(new[] { Color.Gray });
            }

            return _texture;
        }

        public Color GetColor() {
            return _isEnabled ? (_mouseOver ? _colorGroup.Hover : _colorGroup.Normal) : _colorGroup.Disabled;
        }

        // Interface implementations.
        public Rect GetRect() => _rect;
        public void SetRect(Rect rect) => _rect = rect;
        public void OnMouseIn() => _mouseOver = true;
        public void OnMouseOut() => _mouseOver = false;
    }

    public struct ButtonColorGroup {
        public Color Normal;
        public Color Disabled;
        public Color Hover;
        public Color Active;

        public ButtonColorGroup(Color normal, Color disabled, Color hover, Color active) {
            Normal = normal;
            Disabled = disabled;
            Hover = hover;
            Active = active;
        }

        public ButtonColorGroup(Color normal, Color disabled, Color hover) : this (normal, disabled, hover, normal) {}

        public ButtonColorGroup(Color normal, Color disabled) : this (normal, disabled, normal, normal) {}
        public ButtonColorGroup(Color normal) : this (normal, normal, normal, normal) {}        
    }
}