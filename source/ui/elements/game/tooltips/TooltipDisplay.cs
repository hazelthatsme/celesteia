using Microsoft.Xna.Framework;

namespace Celesteia.UI.Elements.Game.Tooltips {
    public class TooltipDisplay : Container
    {
        public TooltipDisplay(Rect rect) : base(rect) {}

        public void MoveTo(Point point) {
            SetRect(GetRect()
                .SetX(AbsoluteUnit.WithValue(point.X))
                .SetY(AbsoluteUnit.WithValue(point.Y))
            );
        }
    }
}