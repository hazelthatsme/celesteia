using System.Collections.Generic;
using System.Diagnostics;
using Celestia.GameInput;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.GUIs {
    public class GUI {
        public Game Game;

        public List<IElement> elements = new List<IElement>();

        public GUI(Game Game) {
            this.Game = Game;
        }

        public virtual void ResolveMouseClick(Point position, MouseButtons buttons) {
            elements.FindAll(x => x.GetType() == typeof(Button)).ForEach(element => {
                Button button = element as Button;

                if (button.GetRect().Contains(position)) {
                    button.Click(position);
                }
            });
        }

        public virtual void LoadContent() {}

        public virtual void Update(GameTime gameTime) {
            if (Input.Mouse.GetMouseUp(MouseButtons.Left)) {
                ResolveMouseClick(Input.Mouse.GetPosition(), MouseButtons.Left);
            }
        }

        // Draw all elements.
        public virtual void Draw(GameTime gameTime) {
            Game.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);

            elements.ForEach(element => {
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