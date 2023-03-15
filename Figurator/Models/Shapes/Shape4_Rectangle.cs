using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using static Figurator.Models.Shapes.PropsN;

namespace Figurator.Models.Shapes {
    public class Shape4_Rectangle: IShape {
        private static readonly PropsN[] props = new[] { PName, PStartDot, PWidth, PHeight, PColor, PThickness, PFillColor };

        /*
         * IShape-часть:
         */

        public PropsN[] Props => props;

        public string Name => "Прямоугольник";

        public Shape? Build(Mapper map) {
            if (map.GetProp(PStartDot) is not SafePoint @start || !@start.Valid) return null;

            if (map.GetProp(PWidth) is not SafeNum @width || !@width.Valid) return null;

            if (map.GetProp(PHeight) is not SafeNum @height || !@height.Valid) return null;

            if (map.GetProp(PColor) is not string @color) return null;

            if (map.GetProp(PFillColor) is not string @fillColor) return null;

            if (map.GetProp(PThickness) is not int @thickness) return null;

            var p = @start.Point;

            return new Rectangle {
                Margin = new(p.X, p.Y, 0, 0), // Грубо и дёшево, но сердито ;'-} Вместо Canvas.Left и Canvas.Top ;'-}}}
                Width = @width.Num,
                Height = @height.Num,
                Stroke = new SolidColorBrush(Color.Parse(@color)),
                Fill = new SolidColorBrush(Color.Parse(@fillColor)),
                StrokeThickness = @thickness
            };
        }
    }
}
