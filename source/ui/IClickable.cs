using Microsoft.Xna.Framework;

namespace Celesteia.UI {
    public interface IClickable : IElement {
        void OnClick(Point position);
    }
}