using Avalonia.Controls.Shapes;
using Avalonia.Media;
using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape5_Ellipse: IShape {
        private static readonly PropsN[] props = new[] { PName, PCenterDot, PHorizDiagonal, PVertDiagonal, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;

        public string Name => "Эллипс";

        public Shape? Build(Mapper map) {
            if (map.GetProp(PCenterDot) is not SafePoint @center || !@center.Valid) return null;

            if (map.GetProp(PWidth) is not SafeNum @width || !@width.Valid) return null;

            if (map.GetProp(PHeight) is not SafeNum @height || !@height.Valid) return null;

            if (map.GetProp(PColor) is not string @color) return null;

            if (map.GetProp(PFillColor) is not string @fillColor) return null;

            if (map.GetProp(PThickness) is not int @thickness) return null;

            var p = @center.Point;
            int w = @width.Num;
            int h = @height.Num;

            return new Ellipse {
                Margin = new(p.X - w/2, p.Y - h/2, 0, 0), // А это совсем читы пошли))) Rectangle отдыхает :D 
                Width = w,
                Height = h,
                Stroke = new SolidColorBrush(Color.Parse(@color)),
                Fill = new SolidColorBrush(Color.Parse(@fillColor)),
                StrokeThickness = @thickness
            };
        }
    }
}
