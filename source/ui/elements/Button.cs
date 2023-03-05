using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI.Elements {
    public class Button : Clickable {
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

        private ClickEvent _onMouseDown = null;
        private ClickEvent _onMouseUp = null;

        public Button SetOnMouseDown(ClickEvent func) {
            _onMouseDown = func;
            return this;
        }

        public Button SetOnMouseUp(ClickEvent func) {
            _onMouseUp = func;
            return this;
        }

        public override void OnMouseDown(MouseButton button, Point position) {
            base.OnMouseDown(button, position);
            _onMouseDown?.Invoke(button, position);
        }

        public override void OnMouseUp(MouseButton button, Point position) {
            base.OnMouseUp(button, position);
            _onMouseUp?.Invoke(button, position);
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

        // https://gamedev.stackexchange.com/a/118255
        private float _colorAmount = 0.0f;
        private bool _prevMouseOver = false;
        private bool _prevClicked = false;
        public override void Update(GameTime gameTime, out bool clickedAnything) {
            clickedAnything = false;
            if (_prevMouseOver != GetMouseOver() || _prevClicked != GetClicked()) _colorAmount = 0.0f;

            _colorAmount += (float)gameTime.ElapsedGameTime.TotalSeconds / 0.5f;

            if (_colorAmount > 1.0f)
                _colorAmount = 0.0f;

            ButtonColor = Color.Lerp(ButtonColor, GetTargetColor(), _colorAmount);

            _prevMouseOver = GetMouseOver();
            _prevClicked = GetClicked();
        }

        Rectangle rectangle;
        public override void Draw(SpriteBatch spriteBatch)
        {
            rectangle = GetRectangle();

            // Draw the button's texture.
            if (_patches != null) ImageUtilities.DrawPatched(spriteBatch, rectangle, _patches, _patchSize, ButtonColor);
            else spriteBatch.Draw(GetTexture(spriteBatch), rectangle, null, ButtonColor);

            TextUtilities.DrawAlignedText(spriteBatch, rectangle, _text);
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            if (_texture == null) {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _texture.SetData(new[] { Color.Gray });
            }

            return _texture;
        }

        private Color GetTargetColor() {
            return ButtonEnabled ? (GetMouseOver() ? (GetClicked() ? _colorGroup.Active : _colorGroup.Hover) : _colorGroup.Normal) : _colorGroup.Disabled;
        }

        public Button Clone() {
            return new Button(GetRect())
                .SetPivotPoint(GetPivot())
                .SetOnMouseDown(_onMouseDown)
                .SetOnMouseUp(_onMouseUp)
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