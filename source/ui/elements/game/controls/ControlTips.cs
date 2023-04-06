using System.Collections.Generic;
using Celesteia.Game.Input;
using Celesteia.Resources;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Celesteia.UI.Elements.Game.Controls {
    public class ControlTips : Container {
        private TextProperties _properties;
        private Dictionary<Keys, string> _keyboardControls = new Dictionary<Keys, string>();
        private List<string> _lines = new List<string>();
        private List<Label> _labels = new List<Label>();

        public ControlTips(Rect rect) : base(rect) {
            _properties = new TextProperties()
                .SetColor(Color.White)
                .SetFont(ResourceManager.Fonts.DEFAULT)
                .SetFontSize(12f)
                .SetTextAlignment(TextAlignment.Left);
        }

        public void SetKeyboardControls(
            params KeyDefinition[] defs
        ) {
            foreach (KeyDefinition def in defs) _keyboardControls.Add(def.GetPositive().Value, def.Name);
            UpdateLines();
        }

        private int lineHeight = 16;
        private Rect LineRect(int line) => new Rect(
            AbsoluteUnit.WithValue(0f),
            AbsoluteUnit.WithValue(line * lineHeight),
            new RelativeUnit(1f, GetRect(), RelativeUnit.Orientation.Horizontal),
            AbsoluteUnit.WithValue(lineHeight)
        );

        private void UpdateLines() {
            _labels.Clear();

            foreach (Keys keys in _keyboardControls.Keys) _lines.Add($"[{keys}] {_keyboardControls[keys]}");

            for (int i = 0; i < _lines.Count; i++) {
                Label label = new Label(LineRect(i - (_lines.Count / 2)))
                    .SetTextProperties(_properties)
                    .SetText(_lines[i]);
                label.SetParent(this);
                _labels.Add(label);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Label l in _labels) l.Draw(spriteBatch);
        }
    }
}