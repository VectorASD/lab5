namespace Figurator.Models {
    public class Mapper {
        public string shapeName = "Линия 1";
        public SafePoint shapeStartDot;
        public SafePoint shapeEndDot;
        public string shapeColor = "Aqua";
        public int shapeThickness = 1;
        public SafePoints shapeDots;
        public string shapeFillColor = "Yellow";
        public string shapeWidth = "200";
        public string shapeHeight = "100";
        public string shapeHorizDiagonal = "100";
        public string shapeVertDiagonal = "200";
        public SafePoint shapeCenterDot;
        public string shapeCommands = "M 0,0 c 0,0 50,0 50,-50 c 0,0 50,0 50,50 h -50 v 50 l -50,-50 Z";

        public Mapper() {
            shapeStartDot = new(50, 50, Update, this);
            shapeEndDot = new(100, 100, Update, this);
            shapeCenterDot = new(150, 150, Update, this);
            shapeDots = new(Update, this);
            shapeDots.Set("50,50 100,100 50,100 100,50");
        }

        private void update() {
            // throw new Exception("Updated!");
        }
        private static void Update(object? me) {
            if (me != null && me is Mapper @map) @map.update();
        }
    }
}
