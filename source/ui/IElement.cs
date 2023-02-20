using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI {
    public interface IElement {
        // Is the element enabled? 
        bool GetEnabled();

        // Set whether the element is enabled.
        bool SetEnabled(bool value);

        // Get the containing rect of the element.
        Rect GetRect();

        // Set the containing rect of the element.
        void SetRect(Rect rect);

        // Get the rectangle with a pivot point.
        Rectangle GetRectangle();

        // Gets the pivot point of the element;
        Vector2 GetPivot();

        // Sets the pivot point of the element;
        void SetPivot(Vector2 pivot);
        
        // Called when the mouse position is within the element's containing rect.
        void OnMouseIn();
        
        // Called when the mouse position is within the element's containing rect.
        void OnMouseOut();

        // Update the element.
        void Update(GameTime gameTime);

        // Draw the element.
        void Draw(SpriteBatch spriteBatch);

        // Get the element's parent.
        IContainer GetParent();

        // Set the element's parent.
        void SetParent(IContainer container);
    }
}