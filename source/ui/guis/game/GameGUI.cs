using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.Game.Components.Items;
using Celesteia.Game.Input;
using Celesteia.Resources;
using Celesteia.UI;
using Celesteia.UI.Elements;
using Celesteia.UI.Elements.Game;
using Celesteia.UI.Elements.Game.Tooltips;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.GUIs.Game {
    public class GameGUI : GUI
    {
        private new GameInstance Game => (GameInstance) base.Game;
        public GameGUI(GameInstance Game) : base(Game, Rect.ScreenFull) { }


        public ItemStack CursorItem;

        private IContainer _pauseMenu;

        private IContainer ItemManagement;

        private InventorySlot _slotTemplate;
        private CraftingRecipeSlot _recipeTemplate;
        private IContainer Hotbar;

        
        private Texture2D slotTexture;
        private TextureAtlas slotPatches;
        private Texture2D windowTexture;
        private Texture2D tooltipTexture;

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

        private IContainer _mousePivot;
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

        private ItemTooltipDisplay _itemDisplay;
        private bool _itemDisplayEnabled = false;
        private CraftingTooltipDisplay _craftingDisplay;
        private bool _craftingDisplayEnabled = false;

        public void SetReferenceInventory(Inventory inventory) {
            _inventory = inventory;

            _slotTemplate.SetReferenceInventory(_inventory);

            LoadHotbar();
            LoadInventoryScreen();
            LoadCraftingScreen();

            State = _state;
        }

        public override void LoadContent(ContentManager Content)
        {
            slotTexture = Content.Load<Texture2D>("sprites/ui/button");
            windowTexture = Content.Load<Texture2D>("sprites/ui/button");
            tooltipTexture = Content.Load<Texture2D>("sprites/ui/window");
            slotPatches = TextureAtlas.Create("patches", slotTexture, 4, 4);

            LoadTooltipDisplays(Content);
            LoadPauseMenu(Content);

            _slotTemplate = new InventorySlot(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(-InventorySlot.SLOT_SPACING),
                AbsoluteUnit.WithValue(InventorySlot.SLOT_SIZE),
                AbsoluteUnit.WithValue(InventorySlot.SLOT_SIZE)
            ))
                .SetTexture(slotTexture)
                .SetPatches(slotPatches, 4)
                .SetTextProperties(new TextProperties()
                    .SetColor(Color.White)
                    .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                    .SetFontSize(16f)
                    .SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right)
                )
                .SetOnMouseIn((item) => {
                    if ((int)State < 1) return;
                    _itemDisplay.SetItem(item);
                    _itemDisplayEnabled = true;
                })
                .SetOnMouseOut(() => _itemDisplayEnabled = false);
            
            _recipeTemplate = new CraftingRecipeSlot(new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(-CraftingRecipeSlot.SLOT_SPACING),
                AbsoluteUnit.WithValue(CraftingRecipeSlot.SLOT_SIZE),
                AbsoluteUnit.WithValue(CraftingRecipeSlot.SLOT_SIZE)
            ))
                .SetTexture(slotTexture)
                .SetPatches(slotPatches, 4)
                .SetTextProperties(new TextProperties()
                    .SetColor(Color.White)
                    .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                    .SetFontSize(16f)
                    .SetTextAlignment(TextAlignment.Bottom | TextAlignment.Right)
                )
                .SetOnMouseIn((recipe) => {
                    if ((int)State < 2) return;
                    //_craftingDisplay.SetRecipe(recipe);
                    _craftingDisplayEnabled = true;
                })
                .SetOnMouseOut(() => _craftingDisplayEnabled = false);

            _slots = new List<InventorySlot>();

            ItemManagement = new Container(Rect.ScreenFull);
            Root.AddChild(ItemManagement);

            LoadHotbar();

            Debug.WriteLine("Loaded Game GUI.");
        }

        public bool Paused { get; private set; }
        public void TogglePause() {
            Paused = !Paused;
            UpdatePauseMenu();
        }

        private void LoadPauseMenu(ContentManager Content) {
            _pauseMenu = new PauseMenu(this, Rect.ScreenFull,
                new Button(new Rect(
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(0f),
                    AbsoluteUnit.WithValue(250f),
                    AbsoluteUnit.WithValue(56f)
                ))
                .SetPivotPoint(new Vector2(.5f))
                .SetTexture(Content.Load<Texture2D>("sprites/ui/button"))
                .MakePatches(4)
                .SetTextProperties(new TextProperties()
                    .SetColor(Color.White)
                    .SetFont(ResourceManager.Fonts.GetFontType("Hobo"))
                    .SetFontSize(24f)
                    .SetTextAlignment(TextAlignment.Center))
                .SetColorGroup(new ButtonColorGroup(Color.White, Color.Black, Color.Violet, Color.DarkViolet))
            );

            Root.AddChild(_pauseMenu);

            UpdatePauseMenu();
        }

        private void LoadTooltipDisplays(ContentManager Content) {
            _mousePivot = new Container(new Rect(
                AbsoluteUnit.WithValue(0f)
            ));
            
            _itemDisplay = new ItemTooltipDisplay(new Rect(
                AbsoluteUnit.WithValue(16f),
                AbsoluteUnit.WithValue(16f),
                AbsoluteUnit.WithValue(256f),
                AbsoluteUnit.WithValue(64f)
            ), tooltipTexture);
            _itemDisplay.SetPivot(new Vector2(0f, 1f));

            _mousePivot.AddChild(_itemDisplay);

            _craftingDisplay = new CraftingTooltipDisplay(new Rect(
                AbsoluteUnit.WithValue(16f),
                AbsoluteUnit.WithValue(16f),
                AbsoluteUnit.WithValue(150f),
                AbsoluteUnit.WithValue(250f)
            ));
            _craftingDisplay.SetPivot(new Vector2(0f, 0f));

            _mousePivot.AddChild(_craftingDisplay);
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
                InventorySlot slot = _slotTemplate.Clone()
                    .SetNewRect(_slotTemplate.GetRect().SetX(AbsoluteUnit.WithValue(i * InventorySlot.SLOT_SIZE + (i * InventorySlot.SLOT_SPACING))))
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
            ), windowTexture, _inventory, remainingSlots, HotbarSlots, _slotTemplate);
            _inventoryScreen.SetPivot(new Vector2(0.5f, 1f));

            pivot.AddChild(_inventoryScreen);
            ItemManagement.AddChild(pivot);
        }

        private void LoadCraftingScreen() {
            int remainingSlots = _inventory.Capacity - HotbarSlots;
            int rows = (remainingSlots / HotbarSlots);

            Container pivot = new Container(new Rect(
                new RelativeUnit(0.5f, ItemManagement.GetRect(), RelativeUnit.Orientation.Horizontal),
                new RelativeUnit(0f, ItemManagement.GetRect(), RelativeUnit.Orientation.Vertical),
                new AbsoluteUnit((HotbarSlots * CraftingRecipeSlot.SLOT_SIZE) + ((HotbarSlots + 1) * CraftingRecipeSlot.SLOT_SPACING)),
                new AbsoluteUnit((rows * CraftingRecipeSlot.SLOT_SIZE) + ((rows + 1) * CraftingRecipeSlot.SLOT_SPACING))
            ));

            _craftingScreen = new CraftingWindow(this, new Rect(
                AbsoluteUnit.WithValue(0f),
                AbsoluteUnit.WithValue(CraftingRecipeSlot.SLOT_SPACING),
                new RelativeUnit(1f, pivot.GetRect(), RelativeUnit.Orientation.Horizontal),
                new RelativeUnit(1f, pivot.GetRect(), RelativeUnit.Orientation.Vertical)
            ), windowTexture, _inventory, _recipeTemplate);
            _craftingScreen.SetPivot(new Vector2(0.5f, 0f));

            pivot.AddChild(_craftingScreen);
            ItemManagement.AddChild(pivot);
        }

        public override void Update(GameTime gameTime, out bool clickedAnything)
        {
            _mousePivot.MoveTo(MouseWrapper.GetPosition());
            _itemDisplay.SetEnabled(_itemDisplayEnabled && (int)_state > 0);
            _craftingDisplay.SetEnabled(_craftingDisplayEnabled && (int)_state > 1);

            base.Update(gameTime, out clickedAnything);
        }

        private Color _slightlyTransparent = new Color(255, 255, 255, 175);
        private Vector2 scale = new Vector2(2f);
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null);

            if (CursorItem != null) Game.SpriteBatch.Draw(CursorItem.Type.Sprite, MouseWrapper.GetPosition().ToVector2(), _slightlyTransparent, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            else {
                _itemDisplay.Draw(Game.SpriteBatch);
                _craftingDisplay.Draw(Game.SpriteBatch);
            }

            Game.SpriteBatch.End();
        }

        public ItemStack GetSelectedItem() {
            return _inventory.GetSlot(HotbarSelection);
        }

        private void UpdateSelected() {
            _slots.ForEach(slot => slot.SetSelected(false));
            _slots[HotbarSelection].SetSelected(true);
        }

        private void UpdatePauseMenu() {
            _pauseMenu.SetEnabled(Paused);
        }
    }

    public enum InventoryScreenState {
        Closed, Inventory, Crafting
    }
}