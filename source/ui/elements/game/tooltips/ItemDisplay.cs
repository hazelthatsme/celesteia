using Celesteia.Resources.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI.Elements.Game.Tooltips {
    public class ItemDisplay : Element {
        public TextureRegion2D Sprite;

        public ItemDisplay(Rect rect) {
            SetRect(rect);
        }

        public ItemDisplay SetItem(ItemType type) {
            Sprite = type.Sprite;
            return this;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GetRectangle(), Color.White, null);
        }
    }
}