using System;
using Celesteia.Resources;
using Celesteia.Resources.Types;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI {
    public class Button : Element, IClickable {
        public Button(Rect rect) {
            SetRect(rect);
        }
        
        public Button SetNewRect(Rect rect) {
            SetRect(rect);
            return this;
        }

        public Button SetPivotPoint(Vector2 pivot) {
            SetPivot(pivot);
            return this;
        }

        // TEXT PROPERTIES

        private TextProperties _text;

        public Button SetTextProperties(TextProperties text) {
            _text = text;
            return this;
        }

        public Button SetText(string text) {
            _text.SetText(text);
            return this;
        }

        // COLOR PROPERTIES

        private ButtonColorGroup _colorGroup = new ButtonColorGroup(Color.White);
        private Color ButtonColor = Color.White;
        private bool ButtonEnabled = true;

        public Button SetColorGroup(ButtonColorGroup colorGroup) {
            _colorGroup = colorGroup;
            return this;
        }

        public Button SetButtonEnabled(bool enabled) {
            ButtonEnabled = enabled;
            return this;
        }

        // CLICKING PROPERTIES

        public delegate void ClickEvent(Point position);
        private ClickEvent _onClick = null;

        public Button SetOnClick(ClickEvent onClick) {
            _onClick = onClick;
            return this;
        }

        // DRAWING PROPERTIES

        private Texture2D _texture;
        private TextureAtlas _patches;
        private int _patchSize;

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

        // Functions

        public void OnClick(Point position) {
            _onClick?.Invoke(position);
        }

        // https://gamedev.stackexchange.com/a/118255
        private float _colorAmount = 0.0f;
        private bool _prevMouseOver = false;
        public override void Update(GameTime gameTime) {
            if (_prevMouseOver != GetMouseOver()) _colorAmount = 0.0f;

            _colorAmount += (float)gameTime.ElapsedGameTime.TotalSeconds / 0.5f;

            if (_colorAmount > 1.0f)
                _colorAmount = 0.0f;

            ButtonColor = Color.Lerp(ButtonColor, GetTargetColor(), _colorAmount);

            _prevMouseOver = GetMouseOver();
        }

        Rectangle r;
        public override void Draw(SpriteBatch spriteBatch)
        {
            r = GetRectangle();

            // Draw the button's texture.
            if (_patches != null) DrawPatched(spriteBatch, r);
            else spriteBatch.Draw(GetTexture(spriteBatch), r, null, ButtonColor);

            TextUtilities.DrawAlignedText(spriteBatch, _text.GetFont(), _text.GetText(), _text.GetColor(), _text.GetAlignment(), r, 24f);
        }

        private int _scaledPatchSize => _patchSize * UIReferences.Scaling;
        private void DrawPatched(SpriteBatch spriteBatch, Rectangle r) {
            int y;

            // Top
            y = r.Y;
            {
                // Top left
                spriteBatch.Draw(_patches.GetRegion(0), new Rectangle(r.X, y, _scaledPatchSize, _scaledPatchSize), ButtonColor);

                // Top center
                spriteBatch.Draw(_patches.GetRegion(1), new Rectangle(r.X + _scaledPatchSize, y, r.Width - (2 * _scaledPatchSize), _scaledPatchSize), ButtonColor);

                // Top right
                spriteBatch.Draw(_patches.GetRegion(2), new Rectangle(r.X + r.Width - _scaledPatchSize, y, _scaledPatchSize, _scaledPatchSize), ButtonColor);
            }

            // Center
            y = r.Y + _scaledPatchSize;
            {
                // Left center
                spriteBatch.Draw(_patches.GetRegion(3), new Rectangle(r.X, y, _scaledPatchSize, r.Height - (2 * _scaledPatchSize)), ButtonColor);

                // Center
                spriteBatch.Draw(_patches.GetRegion(4), new Rectangle(r.X + _scaledPatchSize, y, r.Width - (2 * _scaledPatchSize), r.Height - (2 * _scaledPatchSize)), ButtonColor);

                // Right center
                spriteBatch.Draw(_patches.GetRegion(5), new Rectangle(r.X + r.Width - _scaledPatchSize, y, _scaledPatchSize, r.Height - (2 * _scaledPatchSize)), ButtonColor);
            }

            // Bottom
            y = r.Y + r.Height - _scaledPatchSize;
            {
                // Bottom left
                spriteBatch.Draw(_patches.GetRegion(6), new Rectangle(r.X, y, _scaledPatchSize, _scaledPatchSize), ButtonColor);

                // Bottom center
                spriteBatch.Draw(_patches.GetRegion(7), new Rectangle(r.X + _scaledPatchSize, y, r.Width - (2 * _scaledPatchSize), _scaledPatchSize), ButtonColor);

                // Bottom right
                spriteBatch.Draw(_patches.GetRegion(8), new Rectangle(r.X + r.Width - _scaledPatchSize, y, _scaledPatchSize, _scaledPatchSize), ButtonColor);
            }
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            if (_texture == null) {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _texture.SetData(new[] { Color.Gray });
            }

            return _texture;
        }

        private Color GetTargetColor() {
            return ButtonEnabled ? (GetMouseOver() ? _colorGroup.Hover : _colorGroup.Normal) : _colorGroup.Disabled;
        }

        public Button Clone() {
            return new Button(GetRect())
                .SetPivotPoint(GetPivot())
                .SetOnClick(_onClick)
                .SetTexture(_texture)
                .MakePatches(_patchSize)
                .SetTextProperties(_text)
                .SetColorGroup(_colorGroup);
        }
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