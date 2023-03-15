using Avalonia.Controls.Shapes;
using Avalonia.Media;
using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape6_CompositeFigure: IShape {
        private static readonly PropsN[] props = new[] { PName, PCommands, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;

        public string Name => "Композитка";

        public Shape? Build(Mapper map) {
            if (map.GetProp(PCommands) is not SafeGeometry @commands || !@commands.Valid) return null;

            if (map.GetProp(PColor) is not string @color) return null;

            if (map.GetProp(PFillColor) is not string @fillColor) return null;

            if (map.GetProp(PThickness) is not int @thickness) return null;

            return new Path {
                Data = @commands.Geometry,
                Stroke = new SolidColorBrush(Color.Parse(@color)),
                Fill = new SolidColorBrush(Color.Parse(@fillColor)),
                StrokeThickness = @thickness
            };
        }
    }
}
