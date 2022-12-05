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

        public int GetAbsoluteX() { return (int) Math.Floor(X * Game.GetGameWindow().ClientBounds.Width); }
        public int GetAbsoluteY() { return (int) Math.Floor(Y * Game.GetGameWindow().ClientBounds.Height); }
        public int GetAbsoluteWidth() { return (int) Math.Floor(Width * Game.GetGameWindow().ClientBounds.Width); }
        public int GetAbsoluteHeight() { return (int) Math.Floor(Height * Game.GetGameWindow().ClientBounds.Height); }

        public override Rectangle ToXnaRectangle()
        {
            return new Rectangle(
                GetAbsoluteX(),
                GetAbsoluteY(),
                GetAbsoluteWidth(),
                GetAbsoluteHeight()
            );
        }

        public override Vector2 GetCenter()
        {
            return new Vector2(
                (float) Math.Floor(GetAbsoluteX() + (GetAbsoluteWidth() / 2f)),
                (float) Math.Floor(GetAbsoluteY() + (GetAbsoluteHeight() / 2f))
            );
        }

        public override Vector2 GetSize() {
            return new Vector2(
                GetAbsoluteWidth(),
                GetAbsoluteHeight()
            );
        }
    }
}