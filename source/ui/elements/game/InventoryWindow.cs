namespace Celesteia.UI.Elements.Game {
    public class InventoryWindow : Container {
        private Image background;
        public InventoryWindow(Rect rect) : base(rect) {
            background = new Image(Rect.RelativeFull(rect));
        }
    }
}