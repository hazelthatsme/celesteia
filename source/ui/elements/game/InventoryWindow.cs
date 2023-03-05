using System;
using Celesteia.Game.Components.Items;
using Celesteia.GUIs.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements.Game {
    public class InventoryWindow : Container {
        private Inventory _referenceInventory;
        private Image background;
        private GameGUI _gameGui;

        public InventoryWindow(GameGUI gameGui, Rect rect, Texture2D backgroundImage, Inventory inventory, int slots, int offset, InventorySlot template) : base(rect) {
            _gameGui = gameGui;

            background = new Image(Rect.RelativeFull(rect)).SetTexture(backgroundImage).MakePatches(4).SetColor(Color.White);
            AddChild(background);

            _referenceInventory = inventory;

            AddSlots(slots, offset, template);
        }

        int columns = 9;
        private void AddSlots(int amount, int offset, InventorySlot template) {
            int rows = (int)Math.Ceiling(amount / (double)columns);

            float o = InventorySlot.SLOT_SPACING;
            int i = 0;
            for (int row = 0; row < rows; row++)
                for (int column = 0; column < 9; column++) {
                    if (i > amount) break;

                    int slotNumber = i + offset;
                    InventorySlot slot = template.Clone()
                        .SetNewRect(template.GetRect()
                            .SetX(AbsoluteUnit.WithValue(column * InventorySlot.SLOT_SIZE + (column * InventorySlot.SLOT_SPACING) + o))
                            .SetY(AbsoluteUnit.WithValue(row * InventorySlot.SLOT_SIZE + (row * InventorySlot.SLOT_SPACING) + o))
                        )
                        .SetSlot(slotNumber)
                        .SetOnMouseUp((button, point) => {
                            ItemStack itemInSlot = _referenceInventory.GetSlot(slotNumber);
                            if ((int)_gameGui.State > 0) {
                                _referenceInventory.SetSlot(slotNumber, _gameGui.CursorItem);
                                _gameGui.CursorItem = itemInSlot;
                            }
                        });
                    slot.SetPivot(new Vector2(0f, 0f));

                    i++;

                    AddChild(slot);
                }
        }
    }
}