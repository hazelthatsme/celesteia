using System;
using Celesteia.Resources;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI.Elements.Game.Tooltips {
    public class ItemDisplay : Element {
        public TextureRegion2D Sprite;
        private int _amount;

        public ItemDisplay(Rect rect) {
            SetRect(rect);
        }

        public ItemDisplay SetItem(ItemType type) {
            Sprite = type.Sprite;
            return this;
        }
        public ItemDisplay SetAmount(int amount) {
            _amount = amount;
            return this;
        }

        private Rectangle GetScaledTriangle(Rectangle r, float scale) {
            int newWidth = (int)Math.Round(r.Width * scale);
            int newHeight = (int)Math.Round(r.Height * scale);
            return new Rectangle(
                (int)Math.Round(r.X + ((r.Width - newWidth) / 2f)),
                (int)Math.Round(r.Y + ((r.Height - newHeight) / 2f)),
                newWidth,
                newHeight
            );
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GetRectangle(), Color.White, null);
            if (_amount > 1) TextUtilities.DrawAlignedText(spriteBatch, GetRectangle(), 
                ResourceManager.Fonts.DEFAULT, $"{_amount}", Color.White, TextAlignment.Bottom | TextAlignment.Right, 16f);
        }
    }
}