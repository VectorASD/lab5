using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Figurator.Models;
using Figurator.Views;
using ReactiveUI;
using System;
using System.Net.Http;
using System.Reactive;

namespace Figurator.ViewModels {
    public class MainWindowViewModel: ViewModelBase {
        private UserControl content;
        private int shape_n = 0;
        private readonly Mapper map;

        private readonly UserControl[] contentArray = new UserControl[] {
            new Shape1_UserControl(),
            new Shape2_UserControl(),
            new Shape3_UserControl(),
            new Shape4_UserControl(),
            new Shape5_UserControl(),
            new Shape6_UserControl()
        };

        private string log = "";
        public string Log { get => log; set => this.RaiseAndSetIfChanged(ref log, value + "\n"); }

        private bool is_enabled = true;
        private IBrush add_color = Brushes.White;
        public IBrush AddColor { get => add_color; set => this.RaiseAndSetIfChanged(ref add_color, value); }

        private void Update() {
            bool valid = map.ValidInput();
            // Log += "Update: " + valid;
            is_enabled = valid;
            AddColor = valid ? Brushes.Lime : Brushes.Pink;
        }
        private static void Update(object? inst) {
            if (inst != null && inst is MainWindowViewModel @mwvm) @mwvm.Update();
        }

        public MainWindowViewModel(MainWindow mw) {
            content = contentArray[0];
            map = new(Update, this);
            Update();

            Add = ReactiveCommand.Create<Unit, Unit>(_ => { FuncAdd(); return new Unit(); });
            Clear = ReactiveCommand.Create<Unit, Unit>(_ => { FuncClear(); return new Unit(); });
            Export = ReactiveCommand.Create<string, Unit>(n => { FuncExport(n); return new Unit(); });
            Import = ReactiveCommand.Create<string, Unit>(n => { FuncImport(n); return new Unit(); });

            var canv = mw.Find<Canvas>("canvas");
            var newy = new Line {
                StartPoint = new Point(50, 50),
                EndPoint = new Point(100, 100),
                Stroke = Brushes.Blue,
                StrokeThickness = 1
            };
            canv.Children.Add(newy);
        }

        public int SelectedShape {
            get => shape_n;
            set { shape_n = value; Content = contentArray[value]; Update(); }
        }

        public UserControl Content {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        /*
         * КнопочК:
         */

        private void FuncAdd() {
            if (!is_enabled) return;
            Log += "Add";
            map.Create();
        }
        private void FuncClear() {
            Log += "Clear";
        }
        private void FuncExport(string Type) {
            Log += "Export: " + Type;
        }
        private void FuncImport(string Type) {
            Log += "Import: " + Type;
        }

        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Clear { get; }
        public ReactiveCommand<string, Unit> Export { get; }
        public ReactiveCommand<string, Unit> Import { get; }

        /*
         * Просто параметры фигур:
         */

        public string ShapeName { get => map.shapeName; set { this.RaiseAndSetIfChanged(ref map.shapeName, value); /*map.update();*/ } }
        
        public string ShapeColor { get => map.shapeColor; set { this.RaiseAndSetIfChanged(ref map.shapeColor, value); } }
        public string ShapeFillColor { get => map.shapeFillColor; set { this.RaiseAndSetIfChanged(ref map.shapeFillColor, value); } }
        public int ShapeThickness { get => map.shapeThickness; set { this.RaiseAndSetIfChanged(ref map.shapeThickness, value); } }

        public SafeNum ShapeWidth => map.shapeWidth;
        public SafeNum ShapeHeight => map.shapeHeight; 
        public SafeNum ShapeHorizDiagonal => map.shapeHorizDiagonal;
        public SafeNum ShapeVertDiagonal => map.shapeVertDiagonal;

        public SafePoint ShapeStartDot => map.shapeStartDot;
        public SafePoint ShapeEndDot => map.shapeEndDot;
        public SafePoint ShapeCenterDot => map.shapeCenterDot;
        public SafePoints ShapeDots => map.shapeDots;

        public string ShapeCommands { get => map.shapeCommands; set { this.RaiseAndSetIfChanged(ref map.shapeCommands, value); } }

        /*
         * База цветов
         */

        //private readonly static string[] colors = new Colors().GetType().GetProperties().Select(x => x.Name).ToArray(); // Не проканало :/ CheckBox не выдержал такие объёмы ;'-}
        private readonly static string[] colors = new[] {
            "Yellow", "Blue", "Green", "Red",
            "Orange", "Brown", "Pink", "Aqua",
            "Lime",
            "White", "LightGray", "DarkGray", "Black"
        };
        public static string[] ColorsArr { get => colors; }
    }
}