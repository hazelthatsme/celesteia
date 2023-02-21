using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements {
    public class Label : Element {
        private Texture2D _background;
        private TextProperties _text;

        public Label(Rect rect) {
            SetRect(rect);
        }
        
        public Label SetNewRect(Rect rect) {
            SetRect(rect);
            return this;
        }

        public Label SetPivotPoint(Vector2 pivot) {
            SetPivot(pivot);
            return this;
        }

        public Label SetBackground(Texture2D background) {
            SetTexture(background);
            return this;
        }

        public Label SetText(string text) {
            _text.SetText(text);
            return this;
        }

        public Label SetTextProperties(TextProperties text) {
            _text = text;
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the label's background, if present.
            if (_background != null) spriteBatch.Draw(GetTexture(), GetRectangle(), null, Color.White);

            TextUtilities.DrawAlignedText(spriteBatch, _text.GetFont(), _text.GetText(), _text.GetColor(), _text.GetAlignment(), GetRectangle(), _text.GetFontSize());
        }

        public Texture2D GetTexture() => _background;
        public void SetTexture(Texture2D background) => _background = background;

        public Label Clone() {
            return new Label(GetRect())
                .SetPivotPoint(GetPivot())
                .SetBackground(GetTexture())
                .SetTextProperties(_text);
        }
    }
}