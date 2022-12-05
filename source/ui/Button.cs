using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.UI {
    public class Button {
        public Rect rect;

        public delegate void OnClick(Point position);
        private OnClick onClick;

        private Texture2D texture;

        public string Text { get; private set; }
        
        public Button(Rect rect, OnClick onClick, string text) {
            this.rect = rect;
            this.onClick = onClick;
            this.Text = text;

            this.texture = GetTexture();
        }

        public Button(Rect rect, OnClick onClick) : this (rect, null, "") { }
        public Button(Rect rect) : this (rect, null) { }

        public Texture2D GetTexture() {
            if (this.texture == null) {
                this.texture = new Texture2D(Game.GetSpriteBatch().GraphicsDevice, 1, 1);
                this.texture.SetData(new[] { Color.Gray });
            }

            return this.texture;
        }

        public void Click(Point position) {
            onClick?.Invoke(position);
        }
    }
}