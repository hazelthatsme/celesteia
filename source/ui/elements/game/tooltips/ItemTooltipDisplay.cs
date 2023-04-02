using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements.Game.Tooltips {
    public class ItemTooltipDisplay : TooltipDisplay
    {
        private const float OUTER_SPACING = 16f;
        private const float INNER_SPACING = 8f;
        public readonly Container Content;
        public readonly Label Title;
        public readonly ItemDisplay Item;

        public ItemTooltipDisplay(Rect rect, Texture2D background) : base(rect) {
            AddChild(new Image(Rect.RelativeFull(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(256f + (2 * OUTER_SPACING)),
                AbsoluteUnit.WithValue(32f + (2 * OUTER_SPACING))
            ))).SetTexture(background).MakePatches(4).SetColor(Color.White));

            Content = new Container(new Rect(
                AbsoluteUnit.WithValue(OUTER_SPACING),
                AbsoluteUnit.WithValue(OUTER_SPACING),
                AbsoluteUnit.WithValue(256f),
                AbsoluteUnit.WithValue(32f)
            ));

            Container titleCard = new Container(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(0f),
                new RelativeUnit(1f, Content.GetRect(), RelativeUnit.Orientation.Horizontal),
                AbsoluteUnit.WithValue(32f)
            ));
            titleCard.AddChild(Item = new ItemDisplay(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(32f),
                AbsoluteUnit.WithValue(32f)
            )));
            titleCard.AddChild(Title = new Label(new Rect(
                AbsoluteUnit.WithValue(72f),
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(150f),
                AbsoluteUnit.WithValue(32f)
            )).SetTextProperties(new Properties.TextProperties().Standard().SetTextAlignment(TextAlignment.Left)).SetPivotPoint(new Vector2(0f, 0f)));
            Content.AddChild(titleCard);

            AddChild(Content);

            SetEnabled(false);
        }

        public void SetItem(ItemType type) {
            Item.SetItem(type);
            Title.SetText(type.Name);
        }
    }
}