using Microsoft.Xna.Framework;

namespace Celesteia.UI {
    public interface IClickableElement : IElement {
        void OnClick(Point position);
    }
}