using System;
using Celesteia.Game.Components.Items;
using Celesteia.Resources.Types;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI.Elements.Game {
    public class InventorySlot : Clickable {
        public const float SLOT_SIZE = 64f;
        public const float SLOT_SPACING = 16f;

        public InventorySlot(Rect rect) {
            SetRect(rect);
        }
        
        public InventorySlot SetNewRect(Rect rect) {
            SetRect(rect);
            return this;
        }

        // SELECTION 

        private bool _selected = false;
        public InventorySlot SetSelected(bool selected) {
            _selected = selected;
            return this;
        }

        public bool GetSelected() {
            return _selected;
        }


        // INVENTORY REFERENCE PROPERTIES

        private Inventory _inventory;
        private int _slot;

        public InventorySlot SetReferenceInventory(Inventory inventory) {
            _inventory = inventory;
            return this;
        }
        
        public InventorySlot SetSlot(int slot) {
            _slot = slot;
            return this;
        }

        // DRAWING PROPERTIES

        private Texture2D _texture;
        private TextureAtlas _patches;
        private int _patchSize;

        public InventorySlot SetTexture(Texture2D texture) {
            _texture = texture;
            return this;
        }

        public InventorySlot SetPatches(TextureAtlas patches, int size) {
            if (_texture != null) {
                _patchSize = size;
                _patches = patches;
            }
            return this;
        }

        // TEXT PROPERTIES

        private TextProperties _text;

        public InventorySlot SetTextProperties(TextProperties text) {
            _text = text;
            return this;
        }

        public InventorySlot SetText(string text) {
            _text.SetText(text);
            return this;
        }

        // CLICKING PROPERTIES

        private ClickEvent _onMouseDown = null;
        private ClickEvent _onMouseUp = null;
        public delegate void ItemHoverEvent(ItemType type);
        private ItemHoverEvent _onMouseIn = null;
        private HoverEvent _onMouseOut = null;

        public InventorySlot SetOnMouseDown(ClickEvent func) {
            _onMouseDown = func;
            return this;
        }

        public InventorySlot SetOnMouseUp(ClickEvent func) {
            _onMouseUp = func;
            return this;
        }

        public InventorySlot SetOnMouseIn(ItemHoverEvent func) {
            _onMouseIn = func;
            return this;
        }

        public InventorySlot SetOnMouseOut(HoverEvent func) {
            _onMouseOut = func;
            return this;
        }
        
        public override void OnMouseDown(MouseButton button, Point position) {
            base.OnMouseDown(button, position);
            _onMouseDown?.Invoke(button, position);
        }

        public override void OnMouseUp(MouseButton button, Point position) {
            base.OnMouseUp(button, position);
            _onMouseUp?.Invoke(button, position);
        }
        
        public override void OnMouseIn() {
            base.OnMouseIn();
            if (_inventory.GetSlot(_slot) != null) _onMouseIn?.Invoke(_inventory.GetSlot(_slot).Type);
        }

        public override void OnMouseOut() {
            base.OnMouseOut();
            _onMouseOut?.Invoke();
        }

        private Rectangle GetScaledTriangle(Rectangle r, float scale) {
            int newWidth = (int)Math.Round(r.Width * scale);
            int newHeight = (int)Math.Round(r.Height * scale);
            return new Rectangle(
                (int)Math.Round(r.X + ((r.Width - newWidth) / 2f)),
                (int)Math.Round(r.Y + ((r.Height - newHeight) / 2f)),
                newWidth,
                newHeight
            );
        }

        Rectangle rectangle;
        Rectangle itemRectangle;
        Rectangle textRectangle;
        ItemStack inSlot;
        Color slightlyTransparent = new Color(255, 255, 255, 100);
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_inventory == null) return;

            rectangle = GetRectangle();
            itemRectangle = GetScaledTriangle(rectangle, .6f);
            textRectangle = GetScaledTriangle(rectangle, .4f);

            // Draw the slot's texture.
            if (_patches != null) ImageUtilities.DrawPatched(spriteBatch, rectangle, _patches, _patchSize, _selected ? Color.DarkViolet : Color.White);
            else spriteBatch.Draw(GetTexture(spriteBatch), rectangle, null, Color.White);

            // Draw item if present.
            inSlot = _inventory.GetSlot(_slot);
            if (inSlot != null) {
                spriteBatch.Draw(inSlot.Type.Sprite, itemRectangle, Color.White);
                if (inSlot.Amount > 1) TextUtilities.DrawAlignedText(spriteBatch, textRectangle, _text.GetFont(), $"{inSlot.Amount}", _text.GetColor(), _text.GetAlignment(), _text.GetFontSize());
            } else TextUtilities.DrawAlignedText(spriteBatch, rectangle, _text.GetFont(), $"{_slot + 1}", slightlyTransparent, TextAlignment.Center, 24f);
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            if (_texture == null) {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _texture.SetData(new[] { Color.Gray });
            }

            return _texture;
        }

        public InventorySlot Clone() {
            return new InventorySlot(GetRect())
                .SetReferenceInventory(_inventory)
                .SetOnMouseDown(_onMouseDown)
                .SetOnMouseUp(_onMouseUp)
                .SetOnMouseIn(_onMouseIn)
                .SetOnMouseOut(_onMouseOut)
                .SetSlot(_slot)
                .SetTextProperties(_text)
                .SetTexture(_texture)
                .SetPatches(_patches, _patchSize);
        }
    }
}