using System;
using System.Diagnostics;
using Celesteia.Game.Components.Items;
using Celesteia.GUIs.Game;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI.Elements.Game {
    public class InventorySlot : Clickable {
        public InventorySlot(Rect rect) {
            SetRect(rect);
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

        public delegate void ClickEvent(MouseButton button, Point position);
        private ClickEvent _onMouseDown = null;
        private ClickEvent _onMouseUp = null;

        public InventorySlot SetOnMouseDown(ClickEvent func) {
            _onMouseDown = func;
            return this;
        }

        public InventorySlot SetOnMouseUp(ClickEvent func) {
            _onMouseUp = func;
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_inventory == null) return;

            rectangle = GetRectangle();
            itemRectangle = GetScaledTriangle(rectangle, .6f);
            textRectangle = GetScaledTriangle(rectangle, .6f);

            // Draw the slot's texture.
            if (_patches != null) ImageUtilities.DrawPatched(spriteBatch, rectangle, _patches, _patchSize, _selected ? Color.DarkViolet : Color.White);
            else spriteBatch.Draw(GetTexture(spriteBatch), rectangle, null, Color.White);

            // Draw item if present.
            if (_inventory.GetSlot(_slot) != null) {
                spriteBatch.Draw(_inventory.GetSlot(_slot).Type.Sprite, itemRectangle, Color.White);
                TextUtilities.DrawAlignedText(spriteBatch, textRectangle, _text.GetFont(), _inventory.GetSlot(_slot).Amount + "", _text.GetColor(), _text.GetAlignment(), _text.GetFontSize());
            }
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            if (_texture == null) {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _texture.SetData(new[] { Color.Gray });
            }

            return _texture;
        }
    }
}