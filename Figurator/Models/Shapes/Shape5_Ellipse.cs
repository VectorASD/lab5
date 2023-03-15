using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape5_Ellipse: IShape {
        private static readonly PropsN[] props = new[] { PName, PCenterDot, PHorizDiagonal, PVertDiagonal, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;
    }
}
