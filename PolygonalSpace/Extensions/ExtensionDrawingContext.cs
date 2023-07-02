using System.Windows.Media;

namespace PolygonalSpace.Extension
{
    internal static class ExtensionDrawingContext
    {
        public static void DrawPolygon(this DrawingContext context, Brush brush, Pen pen, PointCollection points)
        {
            PathFigure figure = new() { StartPoint = points[0] };
            figure.Segments.Add(new PolyLineSegment(points, true));

            PathGeometry geometry = new();
            geometry.Figures.Add(figure);

            context.DrawGeometry(brush, pen, geometry);
        }
    }
}