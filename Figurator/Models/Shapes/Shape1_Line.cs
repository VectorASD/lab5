using Avalonia.Controls.Shapes;
using Avalonia.Media;
using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape1_Line: IShape {
        private static readonly PropsN[] props = new[] { PName, PStartDot, PEndDot, PColor, PThickness };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;

        public string Name => "Линия";

        public Shape? Build(Mapper map) {
            if (map.GetProp(PStartDot) is not SafePoint @start || !@start.Valid) return null;

            if (map.GetProp(PEndDot) is not SafePoint @end || !@end.Valid) return null;

            if (map.GetProp(PColor) is not string @color) return null;

            if (map.GetProp(PThickness) is not int @thickness) return null;

            return new Line {
                StartPoint = @start.Point,
                EndPoint = @end.Point,
                Stroke = new SolidColorBrush(Color.Parse(@color)),
                StrokeThickness = @thickness
            };
        }
    }
}
