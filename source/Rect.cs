using System;
using Microsoft.Xna.Framework;

namespace Celestia {
    public class Rect {
        public static Rect Zero = new Rect(0f, 0f, 0f, 0f);
        public static Rect One = new Rect(1f, 1f, 1f, 1f);

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
            return $"{X.ToString("0.00")} {Y.ToString("0.00")} {Width.ToString("0.00")} {Height.ToString("0.00")}";
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

        public virtual Vector2 GetCenter()
        {
            return new Vector2(X + (Width / 2f), Y + (Height / 2f));
        }

        public virtual Vector2 GetSize() {
            return new Vector2(Width, Height);
        }
    }
}