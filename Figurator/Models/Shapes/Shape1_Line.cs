using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape1_Line: IShape {
        private static readonly PropsN[] props = new[] { PName, PStartDot, PEndDot, PColor, PThickness };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;
    }
}
