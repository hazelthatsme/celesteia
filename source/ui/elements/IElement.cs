using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Celesteia.UI.Elements {
    public interface IElement {
        // Is the element enabled? 
        bool GetEnabled();

        // Set whether the element is enabled.
        bool SetEnabled(bool value);

        // Get the containing rect of the element.
        Rect GetRect();

        // Set the containing rect of the element.
        void SetRect(Rect rect);

        // Move to a point.
        void MoveTo(Point point);

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
    	
        // Get if the element has the mouse over it.
        bool GetMouseOver();

        // Update the element.
        void Update(GameTime gameTime, out bool clickedAnything);

        // Draw the element.
        void Draw(SpriteBatch spriteBatch);

        // Get the element's parent.
        IContainer GetParent();

        // Set the element's parent.
        void SetParent(IContainer container);
    }
}