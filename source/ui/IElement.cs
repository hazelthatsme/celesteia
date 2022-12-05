using Microsoft.Xna.Framework.Graphics;

namespace Celestia.UI {
    public interface IElement {
        // Get the containing rect of the element.
        Rect GetRect();

        // Set the containing rect of the element.
        void SetRect(Rect rect);

        // Draw the element.
        void Draw(SpriteBatch spriteBatch);
    }
}