using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Celestia.UI {
    public abstract class Menu {
        public List<Button> buttons = new List<Button>();

        public virtual void ResolveMouseClick(Point position, ButtonState leftButton, ButtonState rightButton) {
            // Loop through all the buttons.
            for (int index = 0; index < buttons.Count; index++)
                // Check if the button's rectangle envelopes/contains the mouse position.
                if (buttons[index].rect.Contains(position)) {
                    // Click the button.
                    buttons[index].Click(position);
                    continue;
                }
        }

        public virtual void Draw() {
            for (int index = 0; index < buttons.Count; index++) {
                Game.GetSpriteBatch().Draw(buttons[index].GetTexture(), buttons[index].rect.ToXnaRectangle(), Color.White);
            }
        }

        public static implicit operator bool(Menu menu)
        {
            return !object.ReferenceEquals(menu, null);
        }
    }
}