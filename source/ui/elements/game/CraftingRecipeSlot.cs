using System;
using System.Diagnostics;
using Celesteia.Game.Components.Items;
using Celesteia.GUIs.Game;
using Celesteia.Resources.Collections;
using Celesteia.UI.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.UI.Elements.Game {
    public class CraftingRecipeSlot : Clickable {
        public const float SLOT_SIZE = 64f;
        public const float SLOT_SPACING = 16f;

        public CraftingRecipeSlot(Rect rect) {
            SetRect(rect);
        }
        
        public CraftingRecipeSlot SetNewRect(Rect rect) {
            SetRect(rect);
            return this;
        }

        // RECIPE REFERENCE PROPERTIES
        private CraftingRecipe _recipe;
        public CraftingRecipeSlot SetRecipe(CraftingRecipe recipe) {
            _recipe = recipe;
            return this;
        }

        // DRAWING PROPERTIES

        private Texture2D _texture;
        private TextureAtlas _patches;
        private int _patchSize;

        public CraftingRecipeSlot SetTexture(Texture2D texture) {
            _texture = texture;
            return this;
        }

        public CraftingRecipeSlot SetPatches(TextureAtlas patches, int size) {
            if (_texture != null) {
                _patchSize = size;
                _patches = patches;
            }
            return this;
        }

        // TEXT PROPERTIES

        private TextProperties _text;

        public CraftingRecipeSlot SetTextProperties(TextProperties text) {
            _text = text;
            return this;
        }

        public CraftingRecipeSlot SetText(string text) {
            _text.SetText(text);
            return this;
        }

        // CLICKING PROPERTIES

        private ClickEvent _onMouseDown = null;
        private ClickEvent _onMouseUp = null;
        public delegate void CraftHoverEvent(CraftingRecipe recipe);
        private CraftHoverEvent _onMouseIn = null;
        private HoverEvent _onMouseOut = null;

        public CraftingRecipeSlot SetOnMouseDown(ClickEvent func) {
            _onMouseDown = func;
            return this;
        }

        public CraftingRecipeSlot SetOnMouseUp(ClickEvent func) {
            _onMouseUp = func;
            return this;
        }

        public CraftingRecipeSlot SetOnMouseIn(CraftHoverEvent func) {
            _onMouseIn = func;
            return this;
        }

        public CraftingRecipeSlot SetOnMouseOut(HoverEvent func) {
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
            if (_recipe != null) _onMouseIn?.Invoke(_recipe);
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_recipe == null) return;

            rectangle = GetRectangle();
            itemRectangle = GetScaledTriangle(rectangle, .6f);
            textRectangle = GetScaledTriangle(rectangle, .4f);

            // Draw the slot's texture.
            if (_patches != null) ImageUtilities.DrawPatched(spriteBatch, rectangle, _patches, _patchSize, Color.White);
            else spriteBatch.Draw(GetTexture(spriteBatch), rectangle, null, Color.White);

            spriteBatch.Draw(_recipe.Result.Type.Sprite, itemRectangle, Color.White);
            TextUtilities.DrawAlignedText(spriteBatch, textRectangle, _text.GetFont(), _recipe.Result.Amount + "", _text.GetColor(), _text.GetAlignment(), _text.GetFontSize());
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch) {
            if (_texture == null) {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _texture.SetData(new[] { Color.Gray });
            }

            return _texture;
        }

        public CraftingRecipeSlot Clone() {
            return new CraftingRecipeSlot(GetRect())
                .SetRecipe(_recipe)
                .SetOnMouseDown(_onMouseDown)
                .SetOnMouseUp(_onMouseUp)
                .SetTextProperties(_text)
                .SetTexture(_texture)
                .SetPatches(_patches, _patchSize);
        }
    }
}