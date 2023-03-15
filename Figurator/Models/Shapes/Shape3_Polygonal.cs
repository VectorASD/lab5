using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape3_Polygonal: IShape {
        private static readonly PropsN[] props = new[] { PName, PDots, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;
    }
}
