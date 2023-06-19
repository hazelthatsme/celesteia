using Celesteia.Resources.Types;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI.Elements.Game.Tooltips {
    public class ItemDisplay : Element {
        public ItemType Item;
        public int Amount;
        public TextProperties Text;

        public ItemDisplay(Rect rect) => SetRect(rect);

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Item.Sprite, GetRectangle(), Color.White, null);
            if (Amount > 1) TextUtilities.DrawAlignedText(spriteBatch, GetRectangle(), Text);
        }
    }
}