using System.Collections.Generic;

namespace Celesteia.UI.Elements {
    public interface IContainer : IElement {
        // Get the element's children.
        List<IElement> GetChildren();

        // Add to the element's children.
        void AddChild(IElement element);
    }
}