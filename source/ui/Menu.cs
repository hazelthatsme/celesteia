using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.UI {
    public class Menu {
        public List<IElement> elements = new List<IElement>();

        public virtual void ResolveMouseClick(Point position, MouseButtonState mouseButtonState) {
            elements.FindAll(x => x.GetType() == typeof(Button)).ForEach(element => {
                Button button = element as Button;

                if (button.GetRect().Contains(position)) {
                    button.Click(position);
                }
            });
        }

        // Draw all elements.
        public virtual void Draw(SpriteBatch spriteBatch) {
            elements.ForEach(element => {
                element.Draw(spriteBatch);
            });
        }

        // If the menu is referred to as a boolean, return whether it is non-null (true) or null (false).
        public static implicit operator bool(Menu menu)
        {
            return !object.ReferenceEquals(menu, null);
        }
    }
}