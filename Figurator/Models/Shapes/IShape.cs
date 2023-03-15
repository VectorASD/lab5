namespace Figurator.Models.Shapes {
    public enum PropsN {
        PName, PColor, PFillColor, PThickness,
        PWidth, PHeight, PHorizDiagonal, PVertDiagonal,
        PStartDot, PEndDot, PCenterDot, PDots,
        PCommands
    }
    internal interface IShape {
        public PropsN[] Props { get; }
    }
}
