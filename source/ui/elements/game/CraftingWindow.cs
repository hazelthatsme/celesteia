using System;
using System.Collections.Generic;
using Celesteia.Game.Components.Items;
using Celesteia.GUIs.Game;
using Celesteia.Resources;
using Celesteia.Resources.Management;
using Celesteia.Resources.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements.Game {
    public class CraftingWindow : Container {
        private Inventory _referenceInventory;
        private Image background;
        private GameGUI _gameGui;

        public CraftingWindow(GameGUI gameGui, Rect rect, Texture2D backgroundImage, Inventory inventory, CraftingRecipeSlot template) : base(rect) {
            _gameGui = gameGui;

            _referenceInventory = inventory;

            background = new Image(Rect.RelativeFull(rect)).SetTexture(backgroundImage).MakePatches(4).SetColor(Color.White);
            AddChild(background);

            AddRecipes(27, template);
        }

        int columns = 9;
        private void AddRecipes(int amountPerPage, CraftingRecipeSlot template) {
            int rows = (int)Math.Ceiling(amountPerPage / (double)columns);

            float o = CraftingRecipeSlot.SLOT_SPACING;
            int index = 0;
            int i = 0;
            for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++) {
                    if (i >= ResourceManager.Recipes.Recipes.Count) break;

                    int slotNumber = i;
                    Recipe recipe = ResourceManager.Recipes.Recipes[index];
                    CraftingRecipeSlot slot = template.Clone()
                        .SetNewRect(template.GetRect()
                            .SetX(AbsoluteUnit.WithValue(column * CraftingRecipeSlot.SLOT_SIZE + (column * CraftingRecipeSlot.SLOT_SPACING) + o))
                            .SetY(AbsoluteUnit.WithValue(row * CraftingRecipeSlot.SLOT_SIZE + (row * CraftingRecipeSlot.SLOT_SPACING) + o))
                        )
                        .SetRecipe(recipe)
                        .SetOnMouseUp((button, point) => {
                            if (button == MonoGame.Extended.Input.MouseButton.Left) {
                                recipe.Craft(_referenceInventory);
                            }
                        });
                    slot.referenceInv = _referenceInventory;
                    slot.SetPivot(new Vector2(0f, 0f));

                    index++;
                    i++;

                    AddChild(slot);
                }
        }
    }
}