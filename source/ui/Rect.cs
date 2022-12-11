using System;
using Microsoft.Xna.Framework;

namespace Celestia.UI {
    public class Rect {
        public static Rect AbsoluteZero = new Rect(AbsoluteUnit.WithValue(0f));
        public static Rect AbsoluteOne = new Rect(AbsoluteUnit.WithValue(1f));

        public IInterfaceUnit X { get; private set; }
        public IInterfaceUnit Y { get; private set; }
        public IInterfaceUnit Width { get; private set; }
        public IInterfaceUnit Height { get; private set; }

        public Rect(IInterfaceUnit uniform) : this (uniform, uniform, uniform, uniform) { }

        public Rect(IInterfaceUnit x, IInterfaceUnit y, IInterfaceUnit width, IInterfaceUnit height) {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public float[] Resolve() {
            return new float[] { X.Resolve(), Y.Resolve(), Width.Resolve(), Height.Resolve() };
        }

        public virtual bool Contains(Point point) {
            return (
                point.X >= this.X.Resolve() && 
                point.Y >= this.Y.Resolve() && 
                point.X <= this.X.Resolve() + this.Width.Resolve() && 
                point.Y <= this.Y.Resolve() + this.Height.Resolve()
            );
        }

        public virtual bool Contains(int x, int y) {
            return Contains(new Point(x, y));
        }

        public override string ToString() {
            return $"{X.Resolve().ToString("0.00")} {Y.Resolve().ToString("0.00")} {Width.Resolve().ToString("0.00")} {Height.Resolve().ToString("0.00")}";
        }

        public virtual Rectangle ToXnaRectangle()
        {
            float[] resolved = this.Resolve();
            return new Rectangle(
                (int) Math.Floor(resolved[0]),
                (int) Math.Floor(resolved[1]),
                (int) Math.Floor(resolved[2]),
                (int) Math.Floor(resolved[3])
            );
        }

        public virtual Vector2 GetCenter()
        {
            float[] resolved = this.Resolve();
            return new Vector2(resolved[0] + (resolved[2] / 2f), resolved[1] + (resolved[3] / 2f));
        }

        public virtual Vector2 GetSize() {
            float[] resolved = this.Resolve();
            return new Vector2(resolved[2], resolved[3]);
        }
    }

    public interface IInterfaceUnit {
        public float Resolve();

        public void SetValue(float value);
    }

    public struct AbsoluteUnit : IInterfaceUnit
    {
        public float value { get; private set; }

        public AbsoluteUnit(float value) {
            this.value = value;
        }

        public static AbsoluteUnit WithValue(float value) {
            return new AbsoluteUnit(value);
        }

        public void SetValue(float value) {
            this.value = value;
        }

        public float Resolve()
        {
            return value;
        }
    }

    public class ScreenSpaceUnit : IInterfaceUnit
    {
        public float value { get; private set; }
        public ScreenSpaceOrientation orientation { get; private set; }

        public ScreenSpaceUnit(float value, ScreenSpaceOrientation orientation) {
            this.value = value;
            this.orientation = orientation;
        }

        public void SetValue(float value) {
            this.value = value;
        }

        public void SetOrientation(ScreenSpaceOrientation orientation) {
            this.orientation = orientation;
        }

        public float Resolve()
        {
            switch (orientation) {
                case ScreenSpaceOrientation.Horizontal:
                    return value * Game.GetGameWindow().ClientBounds.Width;
                case ScreenSpaceOrientation.Vertical:
                    return value * Game.GetGameWindow().ClientBounds.Height;
            }
            return 0f;
        }

        public enum ScreenSpaceOrientation {
            Horizontal, Vertical
        }
    }
}