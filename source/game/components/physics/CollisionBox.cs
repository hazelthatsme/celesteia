using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Celesteia.Game.Components.Physics {
    public class CollisionBox {
        public RectangleF _bounds;
        public RectangleF Bounds { get => _bounds; set => _bounds = value; }

        public List<Vector2> Blocks { get; private set; }

        public CollisionBox(float width, float height) {
            Blocks = new List<Vector2>();
            _bounds = new RectangleF(-width / 2f, -height / 2f, width, height);
        }

        public void Update(Vector2 position) {
            _bounds.X = position.X - (Bounds.Width / 2f);
            _bounds.Y = position.Y - (Bounds.Height / 2f);

            UpdateBlocks();
        }

        public void UpdateBlocks() {
            Blocks.Clear();

            Point topLeft = Bounds.ClosestPointTo(new Point2(Bounds.Center.X - (Bounds.Width / 2f), Bounds.Center.Y - (Bounds.Height / 2f)));

            for (int i = 0; i < Bounds.Width; i++)
                for (int j = 0; j < Bounds.Height; i++) {
                    blocks.Add(new Vector2((0, j));
                }
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