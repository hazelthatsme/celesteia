using System.Collections.Generic;
using System.Diagnostics;
using Celestia.GameInput;
using Celestia.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Celestia.GUIs {
    public class GUI {
        public List<IElement> elements = new List<IElement>();

        public virtual void ResolveMouseClick(Point position, MouseButtons buttons) {
            elements.FindAll(x => x.GetType() == typeof(Button)).ForEach(element => {
                Button button = element as Button;

                if (button.GetRect().Contains(position)) {
                    button.Click(position);
                }
            });
        }

        public virtual void Load(ContentManager contentManager) {}

        public virtual void Update(GameTime gameTime) {}

        // Draw all elements.
        public virtual void Draw(SpriteBatch spriteBatch) {
            elements.ForEach(element => {
                element.Draw(spriteBatch);
            });
        }

        // If the menu is referred to as a boolean, return whether it is non-null (true) or null (false).
        public static implicit operator bool(GUI gui)
        {
            return !object.ReferenceEquals(gui, null);
        }
    }
}