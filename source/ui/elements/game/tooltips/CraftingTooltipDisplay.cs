using Celesteia.Resources.Types;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements.Game.Tooltips {
    public class CraftingTooltipDisplay : TooltipDisplay
    {
        private const float OUTER_SPACING = 16f;
        private const float INNER_SPACING = 8f;
        public readonly Container Content;
        public readonly Label Title;
        public readonly ItemDisplay Item;
        public Container Recipe;

        public CraftingTooltipDisplay(Rect rect, Texture2D background) : base(rect) {
            AddChild(new Image(Rect.RelativeFull(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(256f + (2 * OUTER_SPACING)),
                AbsoluteUnit.WithValue(64f + (1 * INNER_SPACING) + (2 * OUTER_SPACING))
            ))).SetTexture(background).MakePatches(4).SetColor(Color.White));

            Content = new Container(new Rect(
                AbsoluteUnit.WithValue(OUTER_SPACING),
                AbsoluteUnit.WithValue(OUTER_SPACING),
                AbsoluteUnit.WithValue(256f),
                AbsoluteUnit.WithValue(64f + (1 * INNER_SPACING))
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
            )) {
                Text = new TextProperties().Standard().SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right)
            });
            titleCard.AddChild(Title = new Label(new Rect(
                AbsoluteUnit.WithValue(72f),
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(150f),
                AbsoluteUnit.WithValue(32f)
            )).SetTextProperties(new Properties.TextProperties().Standard().SetTextAlignment(TextAlignment.Left)).SetPivotPoint(new Vector2(0f, 0f)));
            Content.AddChild(titleCard);

            Recipe = new Container(new Rect(
                new RelativeUnit(.5f, Content.GetRect(), RelativeUnit.Orientation.Horizontal),
                AbsoluteUnit.WithValue(32f + INNER_SPACING),
                new RelativeUnit(1f, Content.GetRect(), RelativeUnit.Orientation.Horizontal),
                AbsoluteUnit.WithValue(32f)
            ));
            Content.AddChild(Recipe);

            AddChild(Content);

            SetEnabled(false);
        }

        public void SetRecipe(Recipe recipe) {
            Item.Item = recipe.Result.GetItemType();
            Title.SetText(recipe.Result.GetItemType().Name);

            if (Recipe != null) Recipe.Dispose();
            Recipe = new Container(new Rect(
                new RelativeUnit(0f, Content.GetRect(), RelativeUnit.Orientation.Horizontal),
                AbsoluteUnit.WithValue(32f + INNER_SPACING),
                new RelativeUnit(1f, Content.GetRect(), RelativeUnit.Orientation.Horizontal),
                AbsoluteUnit.WithValue(32f)
            ));
            Recipe.SetPivot(new Vector2(0f, 0f));

            for (int i = 0; i < recipe.Ingredients.Count; i++)
                Recipe.AddChild(new ItemDisplay(new Rect(
                    AbsoluteUnit.WithValue((i * INNER_SPACING) + (i * 32)),
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(32f),
                    AbsoluteUnit.WithValue(32f)
                )) {
                    Item = recipe.Ingredients[i].GetItemType(),
                    Amount = recipe.Ingredients[i].Amount,
                    Text = new TextProperties().Standard().SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right)
                });
            
            Content.AddChild(Recipe);
        }
    }
}