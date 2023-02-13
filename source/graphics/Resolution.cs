using Microsoft.Xna.Framework;

namespace Celestia.Graphics {
    public struct Resolution {
        public int Width;
        public int Height;

        public Resolution(int w, int h) {
            Width = w;
            Height = h;
        }

        public Resolution(Rectangle rect) {
            Width = rect.Width;
            Height = rect.Height;
        }
    }
}