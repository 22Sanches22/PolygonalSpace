using PolygonalSpace.GenerationComponents;
using System.Windows;
using System.Windows.Media;

namespace PolygonalSpace.Generators
{
    internal class CirclesGenerator : Generator
    {
        private readonly Dot[,] _dots;


        public CirclesGenerator(VisualContainer space, Dot[,] dots)
            : base(space)
        {
            _dots = dots;
        }

        public override void Generate()
        {
            DrawingVisual visual = new();
            DrawingContext context = visual.RenderOpen();

            for (int i = 0; i < _dots.GetLength(0); i++)
            {
                for (int j = 0; j < _dots.GetLength(1); j++)
                {
                    Dot dot = _dots[i, j];

                    if (dot is null)
                        continue;

                    Brush gradient = new RadialGradientBrush(Color.FromArgb(0, 0, 0, 0), dot.Color);
                    context.DrawEllipse(gradient, null, new Point(dot.X, dot.Y), dot.Radius * 2.5d, dot.Radius * 2.5d);
                }
            }

            _space.AddElement(visual);
            context.Close();
        }
    }
}
