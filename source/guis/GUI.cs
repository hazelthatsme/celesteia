using System.Collections.Generic;
using System.Diagnostics;
using Celesteia.GameInput;
using Celesteia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.GUIs {
    public class GUI {
        public Game Game;

        public List<IElement> Elements = new List<IElement>();

        private Point _mousePosition;

        public GUI(Game Game) {
            this.Game = Game;
        }

        public virtual void ResolveMouseClick(MouseButton button) {
            if (!Game.GUIEnabled) return;

            Elements.FindAll(x => x is IClickableElement).ForEach(element => {
                IClickableElement clickable = element as IClickableElement;

                if (clickable.GetRect().Contains(_mousePosition)) {
                    clickable.OnClick(_mousePosition);
                }
            });
        }

        public virtual void ResolveMouseOver() {
            if (!Game.GUIEnabled) return;

            Elements.ForEach(element => {
                if (element.GetRect().Contains(_mousePosition)) {
                    element.OnMouseIn();
                } else element.OnMouseOut();
            });
        }

        public virtual void LoadContent() {
            Debug.WriteLine("Loaded GUI.");
        }

        public virtual void Update(GameTime gameTime) {
            Elements.ForEach(element => {
                element.Update(gameTime);
            });

            _mousePosition = MouseWrapper.GetPosition();

            if (MouseWrapper.GetMouseDown(MouseButton.Left)) {
                ResolveMouseClick(MouseButton.Left);
            }

            ResolveMouseOver();
        }

        // Draw all elements.
        public virtual void Draw(GameTime gameTime) {
            if (!Game.GUIEnabled) return;
            
            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null, null);

            Elements.ForEach(element => {
                element.Draw(Game.SpriteBatch);
            });

            Game.SpriteBatch.End();
        }

        // If the menu is referred to as a boolean, return whether it is non-null (true) or null (false).
        public static implicit operator bool(GUI gui)
        {
            return !object.ReferenceEquals(gui, null);
        }
    }
}