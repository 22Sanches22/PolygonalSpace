using PolygonalSpace.Extension;
using PolygonalSpace.GenerationComponents;
using System;
using System.Windows;
using System.Windows.Media;

namespace PolygonalSpace.Generators
{
    internal class PolygonsGenerator : Generator
    {
        private readonly Dot[,] _dots;


        public PolygonsGenerator(VisualContainer space, Dot[,] dots)
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
                    if (_dots[i, j] is null)
                        continue;

                    Dot[] dots = new Dot[]
                    {
                        _dots[i, j],
                        _dots[i, j].ConnectedDots[0],
                        _dots[i, j].ConnectedDots[1]
                    };

                    PointCollection points = new();

                    double middlePointX = 0, middlePointY = 0;

                    foreach (var item in dots)
                    {
                        Point coordinates = new(item.X, item.Y);
                        points.Add(coordinates);
                        
                        middlePointX += item.X;
                        middlePointY += item.Y;                
                    }

                    middlePointX /= 3d;
                    middlePointY /= 3d;

                    foreach (var item in dots)
                    {
                        double gradientAngle = Math.Atan2(item.Y - middlePointY, item.X - middlePointX);

                        Brush[] gradients = new Brush[]
                        {
                            new LinearGradientBrush(item.Color, Color.FromArgb(0, 0, 0, 0), gradientAngle) { Opacity = 0.2 },
                            new LinearGradientBrush(Color.FromArgb(0, 0, 0, 0), item.Color, gradientAngle) { Opacity = 0.2 },
                        };

                        context.DrawPolygon(item.X > middlePointX ? gradients[1] : gradients[0], null, points);
                    }
                }
            }

            Space.AddElement(visual);
            context.Close();  
        }
    }
}
