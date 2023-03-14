namespace Figurator.Models {
    public class Mapper {
        public string shapeName = "Линия 1";
        public SafePoint shapeStartDot;
        public SafePoint shapeEndDot;
        public string shapeColor = "Aqua";
        public int shapeThickness = 1;
        public string shapeDots = "50,50 100,100 50,100 100,50";
        public string shapeFillColor = "Yellow";
        public string shapeWidth = "200";
        public string shapeHeight = "100";
        public string shapeHorizDiagonal = "100";
        public string shapeVertDiagonal = "200";
        public SafePoint shapeCenterDot;
        public string shapeCommands = "M 0,0 c 0,0 50,0 50,-50 c 0,0 50,0 50,50 h -50 v 50 l -50,-50 Z";

        // public string ShapeStartDot { get => shapeStartDot.get; set { var v = ""; this.RaiseAndSetIfChanged(ref v, value); } }

        public Mapper() {
            shapeStartDot = new(50, 50, Update, this);
            shapeEndDot = new(100, 100, Update, this);
            shapeCenterDot = new(150, 150, Update, this);
        }

        // public string ShapeName { get => shapeName; set { shapeName = value; update(); } }
        // К сожалению!!! я это не смогу засунуть в this.RaiseAndSetIfChanged(ref map.ShapeName, value) из-за ref...

        private void update() {
            // throw new Exception("Updated!");
        }
        private static void Update(object? me) {
            if (me != null && me is Mapper @map) @map.update();
        }
    }
}
