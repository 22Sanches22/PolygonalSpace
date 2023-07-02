using System.Windows.Media;

namespace PolygonalSpace.GenerationComponents
{
    internal class Dot
    {
        public double X { get; }
        public double Y { get; }
        public double Radius { get; }
        public Color Color { get; }
        public Dot[] ConnectedDots { get; } = new Dot[2];
                                       
        public Dot(double x, double y, double radius, Color color) =>
            (X, Y, Radius, Color) = (x, y, radius, color);
    }
}