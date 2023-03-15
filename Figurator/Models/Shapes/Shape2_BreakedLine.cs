using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape2_BreakedLine: IShape {
        private static readonly PropsN[] props = new[] { PName, PDots, PColor, PThickness };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;
    }
}
