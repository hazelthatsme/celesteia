using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Celesteia.Game.Components.Physics {
    public class CollisionBox {
        public RectangleF _bounds;
        public RectangleF Bounds { get => _bounds; set => _bounds = value; }

        public CollisionBox(RectangleF box) {
            _bounds = box;
        }

        public CollisionBox(float width, float height) {
            _bounds = new RectangleF(-width / 2f, -height / 2f, width, height);
        }

        public void Update(Vector2 position) {
            _bounds.X = position.X - (Bounds.Width / 2f);
            _bounds.Y = position.Y - (Bounds.Height / 2f);
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