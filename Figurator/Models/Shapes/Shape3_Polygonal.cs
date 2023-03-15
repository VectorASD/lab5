using Avalonia.Controls.Shapes;
using Avalonia.Media;
using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape3_Polygonal: IShape {
        private static readonly PropsN[] props = new[] { PName, PDots, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;

        public string Name => "Многоугольник";

        public Shape? Build(Mapper map) {
            if (map.GetProp(PDots) is not SafePoints @dots || !@dots.Valid) return null;

            if (map.GetProp(PColor) is not string @color) return null;

            if (map.GetProp(PFillColor) is not string @fillColor) return null;

            if (map.GetProp(PThickness) is not int @thickness) return null;

            return new Polygon {
                Points = @dots.Points,
                Stroke = new SolidColorBrush(Color.Parse(@color)),
                Fill = new SolidColorBrush(Color.Parse(@fillColor)),
                StrokeThickness = @thickness
            };
        }
    }
}
