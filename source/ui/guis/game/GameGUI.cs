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
        public GameGUI(GameInstance Game) : base(Game, Rect.ScreenFull) { 
         }

        private ItemStack _cursorItem;

        private IContainer ItemManagement;

        private IContainer Hotbar;

        private int hotbarSlots = 9;
        private int hotbarItemSize = 64;
        private int slotSpacing = 10;
        
        private Texture2D slotTexture;
        private TextureAtlas slotPatches;

        private Inventory _inventory;
        private List<InventorySlot> _slots;

        private int selectedHotbarSlot = 0;

        private IContainer _inventoryScreen;
        private IContainer _craftingScreen;

        public InventoryScreenState _state = InventoryScreenState.Inventory;

        public void SetReferenceInventory(Inventory inventory) {
            _inventory = inventory;
            UpdateSlotReferences();

            LoadInventoryScreen();
        }

        public override void LoadContent(ContentManager Content)
        {
            slotTexture = Content.Load<Texture2D>("sprites/ui/button");
            slotPatches = TextureAtlas.Create("patches", slotTexture, 4, 4);

            _slots = new List<InventorySlot>();

            ItemManagement = new Container(Rect.ScreenFull);
            Root.AddChild(ItemManagement);

            LoadHotbar();

            Debug.WriteLine("Loaded Game GUI.");
        }

        private void LoadHotbar() {
            Hotbar = new Container(new Rect(
                new RelativeUnit(0.5f, ItemManagement.GetRect(), RelativeUnit.Orientation.Horizontal),
                new RelativeUnit(1f, ItemManagement.GetRect(), RelativeUnit.Orientation.Vertical),
                AbsoluteUnit.WithValue((hotbarSlots * hotbarItemSize) + ((hotbarSlots - 1) * slotSpacing)),
                AbsoluteUnit.WithValue(hotbarItemSize)
            ));
            Hotbar.SetPivot(new Vector2(0.5f, 1f));

            TextProperties text = new TextProperties()
                .SetColor(Color.White)
                .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                .SetFontSize(16f)
                .SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right);

            for (int i = 0; i < hotbarSlots; i++) {
                int slotNumber = i;
                InventorySlot slot = new InventorySlot(new Rect(
                    AbsoluteUnit.WithValue(i * hotbarItemSize + (i * slotSpacing)),
                    AbsoluteUnit.WithValue(-slotSpacing),
                    AbsoluteUnit.WithValue(hotbarItemSize),
                    AbsoluteUnit.WithValue(hotbarItemSize)
                ))
                    .SetReferenceInventory(_inventory)
                    .SetSlot(slotNumber)
                    .SetTexture(slotTexture)
                    .SetPatches(slotPatches, 4)
                    .SetTextProperties(text)
                    .SetOnMouseUp((button, point) => {
                        if ((int)_state < 1) {
                            selectedHotbarSlot = slotNumber;
                            UpdateSelected();
                        } else {
                            ItemStack itemInSlot = _inventory.GetSlot(slotNumber);

                            _inventory.SetSlot(slotNumber, _cursorItem);
                            _cursorItem = itemInSlot;
                        }
                    });
                slot.SetPivot(new Vector2(0f, 0f));
                slot.SetEnabled(true);

                _slots.Add(slot);
                Hotbar.AddChild(slot);
            }

            UpdateSelected();

            ItemManagement.AddChild(Hotbar);
        }

        private void LoadInventoryScreen() {
            int remainingSlots = _inventory.Capacity - hotbarSlots;
            _inventoryScreen = new InventoryWindow(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(hotbarItemSize - slotSpacing),
                new AbsoluteUnit((hotbarSlots * hotbarItemSize) + ((hotbarSlots - 1) * slotSpacing)),
                new AbsoluteUnit((remainingSlots / hotbarSlots) * hotbarItemSize)
            ));
            _inventoryScreen.SetPivot(new Vector2(0.5f, 1f));

            ItemManagement.AddChild(_inventoryScreen);
        }

        private Color _slightlyTransparent = new Color(255, 255, 255, 175);
        private Vector2 scale = new Vector2(2f);
        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null);
            if (_cursorItem != null) Game.SpriteBatch.Draw(_cursorItem.Type.Sprite, MouseWrapper.GetPosition().ToVector2(), _slightlyTransparent, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            Game.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, out bool clickedAnything)
        {
            UpdateHotbar();

            if (_cursorItem != null) Debug.WriteLine(_cursorItem.ToString());

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

    public enum InventoryScreenState {
        Closed, Inventory, Crafting
    }
}