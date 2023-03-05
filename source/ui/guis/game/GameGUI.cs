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

        public ItemStack CursorItem;

        private IContainer ItemManagement;

        private InventorySlot slotTemplate;
        private IContainer Hotbar;

        
        private Texture2D slotTexture;
        private TextureAtlas slotPatches;

        private Inventory _inventory;
        private List<InventorySlot> _slots;

        private int _hotbarSelection = 0;
        public int HotbarSelection {
            get => _hotbarSelection;
            set {
                _hotbarSelection = MathHelper.Clamp(value, 0, HotbarSlots - 1);
                UpdateSelected();
            }
        }
        public readonly int HotbarSlots = 9;

        private IContainer _inventoryScreen;
        private IContainer _craftingScreen;

        private InventoryScreenState _state;
        public InventoryScreenState State {
            get => _state;
            set {
                _state = value;
                _inventoryScreen.SetEnabled((int)_state > 0);
                _craftingScreen.SetEnabled((int)_state > 1);
            }
        }

        public void SetReferenceInventory(Inventory inventory) {
            _inventory = inventory;

            slotTemplate.SetReferenceInventory(_inventory);

            LoadHotbar();
            LoadInventoryScreen();
            LoadCraftingScreen();

            State = _state;
        }

        public override void LoadContent(ContentManager Content)
        {
            slotTexture = Content.Load<Texture2D>("sprites/ui/button");
            slotPatches = TextureAtlas.Create("patches", slotTexture, 4, 4);

            slotTemplate = new InventorySlot(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(-InventorySlot.SLOT_SPACING),
                AbsoluteUnit.WithValue(InventorySlot.SLOT_SIZE),
                AbsoluteUnit.WithValue(InventorySlot.SLOT_SIZE)
            ))
                .SetReferenceInventory(_inventory)
                .SetTexture(slotTexture)
                .SetPatches(slotPatches, 4)
                .SetTextProperties(new TextProperties()
                    .SetColor(Color.White)
                    .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                    .SetFontSize(16f)
                    .SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right)
                );

            _slots = new List<InventorySlot>();

            ItemManagement = new Container(Rect.ScreenFull);
            Root.AddChild(ItemManagement);

            LoadHotbar();

            Debug.WriteLine("Loaded Game GUI.");
        }

        private void LoadHotbar() {
            if (Hotbar != null) {
                Hotbar.Dispose();
                _slots.Clear();
            }

            Hotbar = new Container(new Rect(
                new RelativeUnit(0.5f, ItemManagement.GetRect(), RelativeUnit.Orientation.Horizontal),
                new RelativeUnit(1f, ItemManagement.GetRect(), RelativeUnit.Orientation.Vertical),
                AbsoluteUnit.WithValue((HotbarSlots * InventorySlot.SLOT_SIZE) + ((HotbarSlots - 1) * InventorySlot.SLOT_SPACING)),
                AbsoluteUnit.WithValue(InventorySlot.SLOT_SIZE)
            ));
            Hotbar.SetPivot(new Vector2(0.5f, 1f));

            for (int i = 0; i < HotbarSlots; i++) {
                int slotNumber = i;
                InventorySlot slot = slotTemplate.Clone()
                    .SetNewRect(slotTemplate.GetRect().SetX(AbsoluteUnit.WithValue(i * InventorySlot.SLOT_SIZE + (i * InventorySlot.SLOT_SPACING))))
                    .SetSlot(slotNumber)
                    .SetOnMouseUp((button, point) => {
                        if ((int)State < 1) {
                            HotbarSelection = slotNumber;
                            UpdateSelected();
                        } else {
                            ItemStack itemInSlot = _inventory.GetSlot(slotNumber);

                            _inventory.SetSlot(slotNumber, CursorItem);
                            CursorItem = itemInSlot;
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
            int remainingSlots = _inventory.Capacity - HotbarSlots;
            int rows = (remainingSlots / HotbarSlots);

            Container pivot = new Container(new Rect(
                new RelativeUnit(0.5f, ItemManagement.GetRect(), RelativeUnit.Orientation.Horizontal),
                new RelativeUnit(1f, ItemManagement.GetRect(), RelativeUnit.Orientation.Vertical),
                new AbsoluteUnit((HotbarSlots * InventorySlot.SLOT_SIZE) + ((HotbarSlots + 1) * InventorySlot.SLOT_SPACING)),
                new AbsoluteUnit((rows * InventorySlot.SLOT_SIZE) + ((rows + 1) * InventorySlot.SLOT_SPACING))
            ));

            _inventoryScreen = new InventoryWindow(this, new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(-(InventorySlot.SLOT_SIZE + (2 * InventorySlot.SLOT_SPACING))),
                new RelativeUnit(1f, pivot.GetRect(), RelativeUnit.Orientation.Horizontal),
                new RelativeUnit(1f, pivot.GetRect(), RelativeUnit.Orientation.Vertical)
            ), Game.Content.Load<Texture2D>("sprites/ui/button"), _inventory, remainingSlots, HotbarSlots, slotTemplate);
            _inventoryScreen.SetPivot(new Vector2(0.5f, 1f));

            pivot.AddChild(_inventoryScreen);
            ItemManagement.AddChild(pivot);
        }

        private void LoadCraftingScreen() {
            _craftingScreen = new Container(Rect.AbsoluteZero);
        }

        private Color _slightlyTransparent = new Color(255, 255, 255, 175);
        private Vector2 scale = new Vector2(2f);
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null);
            if (CursorItem != null) Game.SpriteBatch.Draw(CursorItem.Type.Sprite, MouseWrapper.GetPosition().ToVector2(), _slightlyTransparent, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            Game.SpriteBatch.End();
        }

        public ItemStack GetSelectedItem() {
            return _inventory.GetSlot(HotbarSelection);
        }

        private void UpdateSelected() {
            _slots.ForEach(slot => slot.SetSelected(false));
            _slots[HotbarSelection].SetSelected(true);
        }
    }

    public enum InventoryScreenState {
        Closed, Inventory, Crafting
    }
}