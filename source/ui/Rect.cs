using System;
using Microsoft.Xna.Framework;

namespace Celesteia.UI {
    public struct Rect {
        public static Rect AbsoluteZero = new Rect(AbsoluteUnit.WithValue(0f));
        public static Rect AbsoluteOne = new Rect(AbsoluteUnit.WithValue(1f));
        public static Rect ScreenFull = new Rect(
            new ScreenSpaceUnit(0f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
            new ScreenSpaceUnit(0f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical),
            new ScreenSpaceUnit(1f, ScreenSpaceUnit.ScreenSpaceOrientation.Horizontal),
            new ScreenSpaceUnit(1f, ScreenSpaceUnit.ScreenSpaceOrientation.Vertical)
        );
        public static Rect RelativeFull(Rect parent) => new Rect(
            new RelativeUnit(0f, parent, RelativeUnit.Orientation.Horizontal),
            new RelativeUnit(0f, parent, RelativeUnit.Orientation.Vertical),
            new RelativeUnit(1f, parent, RelativeUnit.Orientation.Horizontal),
            new RelativeUnit(1f, parent, RelativeUnit.Orientation.Vertical)
        );

        public IInterfaceUnit X;
        public IInterfaceUnit Y;
        public IInterfaceUnit Width;
        public IInterfaceUnit Height;

        public Rect(IInterfaceUnit uniform) : this (uniform, uniform, uniform, uniform) { }

        public Rect(IInterfaceUnit x, IInterfaceUnit y, IInterfaceUnit width, IInterfaceUnit height) {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Rect SetX(IInterfaceUnit x) {
            X = x;
            return this;
        }

        public Rect SetY(IInterfaceUnit y) {
            Y = y;
            return this;
        }

        public Rect SetWidth(IInterfaceUnit w) {
            Width = w;
            return this;
        }

        public Rect SetHeight(IInterfaceUnit h) {
            Height = h;
            return this;
        }

        public float[] Resolve() {
            return new float[] { X.Resolve(), Y.Resolve(), Width.Resolve(), Height.Resolve() };
        }

        public bool Contains(Point point) {
            return (
                point.X >= this.X.Resolve() && 
                point.Y >= this.Y.Resolve() && 
                point.X <= this.X.Resolve() + this.Width.Resolve() && 
                point.Y <= this.Y.Resolve() + this.Height.Resolve()
            );
        }

        public bool Contains(int x, int y) {
            return Contains(new Point(x, y));
        }

        public override string ToString() {
            return $"{X.Resolve().ToString("0.00")} {Y.Resolve().ToString("0.00")} {Width.Resolve().ToString("0.00")} {Height.Resolve().ToString("0.00")}";
        }

        public Rectangle ResolveRectangle()
        {
            float[] resolved = this.Resolve();
            return new Rectangle(
                (int) Math.Floor(resolved[0]),
                (int) Math.Floor(resolved[1]),
                (int) Math.Floor(resolved[2]),
                (int) Math.Floor(resolved[3])
            );
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

    public struct ScreenSpaceUnit : IInterfaceUnit
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
            if (UIReferences.gameWindow != null) {
                switch (orientation) {
                    case ScreenSpaceOrientation.Horizontal:
                        return value * UIReferences.gameWindow.ClientBounds.Width;
                    case ScreenSpaceOrientation.Vertical:
                        return value * UIReferences.gameWindow.ClientBounds.Height;
                }
            }
            return 0f;
        }

        public enum ScreenSpaceOrientation {
            Horizontal, Vertical
        }
    }

    public struct RelativeUnit : IInterfaceUnit
    {
        public float value { get; private set; }
        public Rect parent { get; private set; }
        public Orientation orientation { get; private set; }

        public RelativeUnit(float value, Rect parent, Orientation orientation) {
            this.value = value;
            this.parent = parent;
            this.orientation = orientation;
        }

        public void SetValue(float value) {
            this.value = value;
        }

        public void SetOrientation(Orientation orientation) {
            this.orientation = orientation;
        }

        public float Resolve()
        {
            switch (orientation) {
                case Orientation.Horizontal:
                    return value * parent.Resolve()[2];
                case Orientation.Vertical:
                    return value * parent.Resolve()[3];
                default:
                    return 0f;
            }
        }

        public enum Orientation {
            Horizontal, Vertical
        }
    }
}