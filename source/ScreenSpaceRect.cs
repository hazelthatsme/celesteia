using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Celestia {
    public class ScreenSpaceRect : Rect
    {
        public ScreenSpaceRect(float x, float y, float width, float height) : base(x, y, width, height)
        {
            
        }

        public override bool Contains(Point point) {
            float screenSpaceX = point.X / (float) Game.GetGameWindow().ClientBounds.Width;
            float screenSpaceY = point.Y / (float) Game.GetGameWindow().ClientBounds.Height;

            Debug.WriteLine(screenSpaceX + ", " + screenSpaceY);

            return (
                screenSpaceX >= this.X && 
                screenSpaceY >= this.Y && 
                screenSpaceX <= this.X + this.Width && 
                screenSpaceY <= this.Y + this.Height
            );
        }

        public override bool Contains(int x, int y) {
            return Contains(new Point(x, y));
        }

        public override Rectangle ToXnaRectangle()
        {
            int absoluteX = (int) Math.Floor(X * Game.GetGameWindow().ClientBounds.Width);
            int absoluteY = (int) Math.Floor(Y * Game.GetGameWindow().ClientBounds.Height);
            int absoluteWidth = (int) Math.Floor(Width * Game.GetGameWindow().ClientBounds.Width);
            int absoluteHeight = (int) Math.Floor(Height * Game.GetGameWindow().ClientBounds.Height);

            return new Rectangle(
                absoluteX,
                absoluteY,
                absoluteWidth,
                absoluteHeight
            );
        }
    }
}