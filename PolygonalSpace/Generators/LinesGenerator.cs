using PolygonalSpace.Extension;
using PolygonalSpace.GenerationComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PolygonalSpace.Generators
{
    internal class LinesGenerator : Generator
    {
        private readonly Range _thicknessRange;
        private readonly Dot[,] _dots;
        private readonly Range _dotsRadiusRange;


        public LinesGenerator(VisualContainer space, Range thicknessRange, Dot[,] dots, Range dotsRadiusRange)
            : base(space)
        {
            (_thicknessRange, _dots, _dotsRadiusRange) = (thicknessRange, dots, dotsRadiusRange);
        }

        public override void Generate()
        {
            Random random = new();

            int dotCountY = _dots.GetLength(0),
                dotCountX = _dots.GetLength(1);

            DrawingVisual visual = new();
            DrawingContext context = visual.RenderOpen();
            
            for (int i = 0; i < dotCountY; i++)
            {
                for (int j = 0; j < dotCountX; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Dot dot1 = _dots[i, j];

                        if (dot1 is null)
                            continue;

                        List<Dot> dot2Options = new();

                        if (i > 0 && j > 0)
                            dot2Options.Add(_dots[i - 1, j - 1]);

                        if (i > 0 && j < dotCountX - 1)
                            dot2Options.Add(_dots[i - 1, j + 1]);

                        if (i < dotCountY - 1 && j > 0)
                            dot2Options.Add(_dots[i + 1, j - 1]);

                        if (i < dotCountY - 1 && j < dotCountX - 1)
                            dot2Options.Add(_dots[i + 1, j + 1]);

                        Dot dot2 = dot2Options[random.Next(0, dot2Options.Count)];
                        dot1.ConnectedDots[k] = dot2;

                        if (dot1.X > dot2.X)
                        {
                            Dot temp = dot1;
                            dot1 = dot2;
                            dot2 = temp;
                        }

                        Color color1 = dot1.Color, color2 = dot2.Color;

                        double gradientAngle = Math.Atan2(dot2.Y - dot1.Y, dot2.X - dot1.X);

                        double zoomPercentageDot1 = CalculateZoomPercentageDot(dot1.Radius),
                               zoomPercentageDot2 = CalculateZoomPercentageDot(dot2.Radius);

                        double initialLineThickness = CalculateThicknessLine(zoomPercentageDot1),
                               finalLineThickness = CalculateThicknessLine(zoomPercentageDot2);

                        PointCollection points = new()
                        {
                            new Point(dot1.X, dot1.Y - initialLineThickness / 2d),
                            new Point(dot1.X, dot1.Y + initialLineThickness / 2d),
                            new Point(dot2.X, dot2.Y + finalLineThickness / 2d),
                            new Point(dot2.X, dot2.Y - finalLineThickness / 2d)
                        };

                        context.DrawPolygon(new LinearGradientBrush(color1, color2, gradientAngle), null, points);
                    }
                }
            }

            Space.AddElement(visual);
            context.Close();
        }

        private double CalculateZoomPercentageDot(double radius) =>
            100d / (_dotsRadiusRange.Max - _dotsRadiusRange.Min) * (radius - _dotsRadiusRange.Min);

        private double CalculateThicknessLine(double zoomPercentage) =>
            (_thicknessRange.Max - _thicknessRange.Min) / 100d * zoomPercentage + _thicknessRange.Min;
    }
}
