using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape4_Rectangle: IShape {
        private static readonly PropsN[] props = new[] { PName, PStartDot, PWidth, PHeight, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;
    }
}
