using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI {
    public interface IElement {
        // Get the containing rect of the element.
        Rect GetRect();

        // Set the containing rect of the element.
        void SetRect(Rect rect);
        
        // Called when the mouse position is within the element's containing rect.
        void OnMouseIn();
        
        // Called when the mouse position is within the element's containing rect.
        void OnMouseOut();

        // Update the element.
        void Update(GameTime gameTime);

        // Draw the element.
        void Draw(SpriteBatch spriteBatch);
    }
}