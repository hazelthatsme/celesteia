using System;
using Microsoft.Xna.Framework;

namespace Celestia {
    public class Rect {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public Rect(float x, float y, float width, float height) {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public virtual bool Contains(Point point) {
            return (
                point.X >= this.X && 
                point.Y >= this.Y && 
                point.X <= this.X + this.Width && 
                point.Y <= this.Y + this.Height
            );
        }

        public virtual bool Contains(int x, int y) {
            return Contains(new Point(x, y));
        }

        public override string ToString() {
            return $"{{{X},{Y},{Width},{Height}}}";
        }

        public virtual Rectangle ToXnaRectangle()
        {
            return new Rectangle(
                (int) Math.Floor(X),
                (int) Math.Floor(Y),
                (int) Math.Floor(Width),
                (int) Math.Floor(Height)
            );
        }
    }
}