using Celesteia.Resources;
using Celesteia.Resources.Management;
using Microsoft.Xna.Framework;

namespace Celesteia.UI.Properties {
    public struct TextProperties {
        private string _text;
        private FontType _font;
        private Color _textColor;
        private float _fontSize;
        private TextAlignment _textAlignment;

        public TextProperties Standard() {
            _text = "";
            _font = ResourceManager.Fonts.DEFAULT;
            _textColor = Color.White;
            _fontSize = 16f;
            _textAlignment = TextAlignment.Center;

            return this;
        }

        public TextProperties SetText(string text) {
            _text = text;
            return this;
        }
        public string GetText() => _text;

        public TextProperties SetFont(FontType font) {
            _font = font;
            return this;
        }
        public FontType GetFont() => _font;

        public TextProperties SetColor(Color textColor) {
            _textColor = textColor;
            return this;
        }
        public Color GetColor() => _textColor;
        
        public TextProperties SetFontSize(float fontSize) {
            _fontSize = fontSize;
            return this;
        }
        public float GetFontSize() => _fontSize;

        public TextProperties SetTextAlignment(TextAlignment textAlignment) {
            _textAlignment = textAlignment;
            return this;
        }
        public TextAlignment GetAlignment() => _textAlignment;

        public TextProperties Clone() {
            return new TextProperties()
                .SetColor(_textColor)
                .SetFont(_font)
                .SetFontSize(_fontSize)
                .SetText(_text)
                .SetTextAlignment(_textAlignment);
        }
    }
}