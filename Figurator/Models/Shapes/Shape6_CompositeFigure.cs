using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape6_CompositeFigure: IShape {
        private static readonly PropsN[] props = new[] { PName, PCommands, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;
    }
}
