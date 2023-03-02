using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Input;
using Celesteia.Resources;
using Celesteia.UI;
using Celesteia.UI.Elements;
using Celesteia.UI.Elements.Game;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Content;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.GUIs.Game {
    public class GameGUI : GUI
    {
        private new GameInstance Game => (GameInstance) base.Game;
        public GameGUI(GameInstance Game) : base(Game, Rect.ScreenFull) { }

        private ItemStack _cursorItem;

        private IContainer Hotbar;

        private int hotbarSlots = 9;
        private int hotbarItemSize = 64;
        private int slotSpacing = 10;
        
        private Texture2D slotTexture;
        private TextureAtlas slotPatches;

        private Inventory _inventory;
        private List<InventorySlot> _slots;

        private int selectedHotbarSlot = 0;

        public void SetReferenceInventory(Inventory inventory) {
            _inventory = inventory;
            UpdateSlotReferences();
        }

        public override void LoadContent(ContentManager Content)
        {
            slotTexture = Content.Load<Texture2D>("sprites/ui/button");
            slotPatches = TextureAtlas.Create("patches", slotTexture, 4, 4);

            _slots = new List<InventorySlot>();

            LoadHotbar();

            Debug.WriteLine("Loaded Game GUI.");
        }

        private void LoadHotbar() {
            Hotbar = new Container(new Rect(
                new ScreenSpaceUnit(0.5f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
                new ScreenSpaceUnit(1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
                AbsoluteUnit.WithValue((hotbarSlots * hotbarItemSize) + ((hotbarSlots - 1) * slotSpacing)),
                AbsoluteUnit.WithValue(hotbarItemSize)
            ));
            Hotbar.SetPivot(new Vector2(0.5f, 0f));
            Debug.WriteLine(Hotbar.GetEnabled());

            TextProperties text = new TextProperties()
                .SetColor(Color.White)
                .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                .SetFontSize(14f)
                .SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right);

            for (int i = 0; i < hotbarSlots; i++) {
                int slotNumber = i;
                InventorySlot slot = new InventorySlot(new Rect(
                    AbsoluteUnit.WithValue(i * hotbarItemSize + (i * slotSpacing)),
                    AbsoluteUnit.WithValue(-20f),
                    AbsoluteUnit.WithValue(hotbarItemSize),
                    AbsoluteUnit.WithValue(hotbarItemSize)
                ))
                    .SetReferenceInventory(_inventory)
                    .SetSlot(slotNumber)
                    .SetTexture(slotTexture)
                    .SetPatches(slotPatches, 4)
                    .SetTextProperties(text)
                    .SetOnMouseUp((button, point) => { selectedHotbarSlot = slotNumber; UpdateSelected(); });
                slot.SetPivot(new Vector2(0f, 1f));
                slot.SetEnabled(true);

                _slots.Add(slot);
                Hotbar.AddChild(slot);
            }

            UpdateSelected();

            Root.AddChild(Hotbar);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, out bool clickedAnything)
        {
            UpdateHotbar();

            Root.Update(gameTime, out clickedAnything);
        }

        public ItemStack GetSelectedItem() {
            return _inventory.GetSlot(selectedHotbarSlot);
        }

        private void UpdateHotbar() {
            if (!KeyboardWrapper.GetKeyHeld(Keys.LeftControl) && MouseWrapper.GetScrollDelta() != 0f) {
                int change = MouseWrapper.GetScrollDelta() > 0f ? -1 : 1;

                selectedHotbarSlot += change;
                if (selectedHotbarSlot < 0) selectedHotbarSlot = hotbarSlots - 1;
                if (selectedHotbarSlot >= hotbarSlots) selectedHotbarSlot = 0;

                UpdateSelected();
            }
        }

        private void UpdateSelected() {
            _slots.ForEach(slot => slot.SetSelected(false));
            _slots[selectedHotbarSlot].SetSelected(true);
        }

        private void UpdateSlotReferences() {
            _slots.ForEach(slot => slot.SetReferenceInventory(_inventory));
        }
    }
}