using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Figurator.Models;
using Figurator.Views;
using ReactiveUI;

namespace Figurator.ViewModels {
    public class MainWindowViewModel: ViewModelBase {
        private UserControl content;
        private readonly UserControl[] contentArray = new UserControl[] {
            new Shape1_UserControl(),
            new Shape2_UserControl(),
            new Shape3_UserControl(),
            new Shape4_UserControl(),
            new Shape5_UserControl(),
            new Shape6_UserControl()
        };

        public MainWindowViewModel(MainWindow mw) {
            content = contentArray[0];
            var canv = mw.Find<Canvas>("canvas");
            var newy = new Line();
            newy.StartPoint = new Point(50, 50);
            newy.EndPoint = new Point(100, 100);
            newy.Stroke = new SolidColorBrush(Colors.Blue);
            newy.StrokeThickness = 1;
            canv.Children.Add(newy);
        }

        int shape_n = 0;
        public int SelectedShape {
            get => shape_n;
            set { shape_n = value; Content = contentArray[value]; }
        }

        public UserControl Content {
            get => content;
            set {
                this.RaiseAndSetIfChanged(ref content, value);
            }
        }



        private readonly Mapper map = new();

        public string ShapeName { get => map.shapeName; set { this.RaiseAndSetIfChanged(ref map.shapeName, value); /*map.update();*/ } }
        public SafePoint ShapeStartDot => map.shapeStartDot;
        public SafePoint ShapeEndDot => map.shapeEndDot;
        public string ShapeColor { get => map.shapeColor; set { this.RaiseAndSetIfChanged(ref map.shapeColor, value); } }
        public int ShapeThickness { get => map.shapeThickness; set { this.RaiseAndSetIfChanged(ref map.shapeThickness, value); } }
        public SafePoints ShapeDots => map.shapeDots;
        public string ShapeFillColor { get => map.shapeFillColor; set { this.RaiseAndSetIfChanged(ref map.shapeFillColor, value); } }
        public string ShapeWidth { get => map.shapeWidth; set { this.RaiseAndSetIfChanged(ref map.shapeWidth, value); } }
        public string ShapeHeight { get => map.shapeHeight; set { this.RaiseAndSetIfChanged(ref map.shapeHeight, value); } }
        public string ShapeHorizDiagonal { get => map.shapeHorizDiagonal; set { this.RaiseAndSetIfChanged(ref map.shapeHorizDiagonal, value); } }
        public string ShapeVertDiagonal { get => map.shapeVertDiagonal; set { this.RaiseAndSetIfChanged(ref map.shapeVertDiagonal, value); } }
        public SafePoint ShapeCenterDot => map.shapeCenterDot;
        public string ShapeCommands { get => map.shapeCommands; set { this.RaiseAndSetIfChanged(ref map.shapeCommands, value); } }



        //private readonly string[] colors = new Colors().GetType().GetProperties().Select(x => x.Name).ToArray(); // Не проканало :/ CheckBox не выдержал такие объёмы ;'-}
        private readonly string[] colors = new[] {
            "Yellow",
            "Blue",
            "Green",
            "Red",
            "Orange",
            "Brown",
            "Pink",
            "Aqua",
            "Lime",
            "White",
            "LightGray",
            "DarkGray",
            "Black"
        };
        public string[] ColorsArr { get => colors; }
    }
}