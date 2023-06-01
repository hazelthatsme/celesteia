using System;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Components.Physics {
    public class CollisionBox {
        public RectangleF _bounds;
        public RectangleF Bounds { get => _bounds; set => _bounds = value; }
        private Rectangle _rounded;
        public Rectangle Rounded { get => _rounded; }

        public CollisionBox(float width, float height) {
            _bounds = new RectangleF(-width / 2f, -height / 2f, width, height);
            _rounded = Round(_bounds);
        }

        public void Update(Vector2 position) {
            _bounds.X = position.X - (Bounds.Width / 2f);
            _bounds.Y = position.Y - (Bounds.Height / 2f);
            _rounded = Round(_bounds);
        }

        public Rectangle Round(RectangleF floatRect) {
            return new Rectangle((int)MathF.Floor(floatRect.X), (int)MathF.Floor(floatRect.Y), (int)MathF.Ceiling(floatRect.Width), (int)MathF.Ceiling(floatRect.Height));
        }

        public RectangleF Intersection(CollisionBox other) {
            return Intersection(other.Bounds);
        }

        public RectangleF Intersection(RectangleF other) {
            RectangleF intersection = other.Intersection(Bounds);
            return intersection;
        }
    }
}