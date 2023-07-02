using PolygonalSpace.GenerationComponents;
using System;
using System.Windows;
using System.Windows.Media;

namespace PolygonalSpace.Generators
{
    internal class DotsGenerator : Generator
    {
        private readonly Point _startPoint;
        private readonly Range _radiusRange;
        private readonly Color _color;      
        private readonly Sector[,] _sectors;
        private readonly Dot[,] _dots;


        public DotsGenerator(VisualContainer space, Color color, Range radiusRange, Point startPoint, Sector[,] sectors)
            : base(space)
        {
            (_color, _radiusRange, _startPoint, _sectors) = (color, radiusRange, startPoint, sectors);
            _dots = new Dot[_sectors.GetLength(0), _sectors.GetLength(1)];
        }

        public override void Generate()
        {
            Random random = new();

            double x = _startPoint.X, y = _startPoint.Y;

            DrawingVisual visual = new();
            DrawingContext context = visual.RenderOpen();

            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    if (!_sectors[i, j].SectorFillAbility)
                    {
                        x += Sector.Size;
                        continue;
                    }

                    double dotRadius = (_radiusRange.Max - _radiusRange.Min) * random.NextDouble() + _radiusRange.Min,
                           dotPosX = GetRandomDotPositionInSector(dotRadius, random.NextDouble()) + x,
                           dotPosY = GetRandomDotPositionInSector(dotRadius, random.NextDouble()) + y;

                    double zoomPercentage = 100d / (_radiusRange.Max - _radiusRange.Min) * (dotRadius - _radiusRange.Min);
                    Color color = Color.FromRgb(GetValueFromPercentage(_color.R, zoomPercentage),
                                                GetValueFromPercentage(_color.G, zoomPercentage),
                                                GetValueFromPercentage(_color.B, zoomPercentage));

                    _dots[i, j] = new Dot(dotPosX, dotPosY, dotRadius, color);

                    context.DrawEllipse(new SolidColorBrush(color), null, new Point(dotPosX, dotPosY), dotRadius, dotRadius);

                    x += Sector.Size;
                }

                x = _startPoint.X;
                y += Sector.Size;
            }

            _space.AddElement(visual);
            context.Close();
        }

        public Dot[,] GetDots() => _dots;
        public Range GetRadiusRange() => _radiusRange;

        private static double GetRandomDotPositionInSector(double dotRadius, double randomDouble) =>
            (Sector.Size - dotRadius * 2) * randomDouble + dotRadius;

        private byte GetValueFromPercentage(byte maxValue, double percent) =>
            (byte)Math.Round(maxValue / 100d * percent);
    }
}