using PolygonalSpace.GenerationComponents;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace PolygonalSpace.Generators
{
    internal class SectorsGenerator : Generator
    {
        private readonly int _sectorSize;
        private readonly Point _startPoint;
        private readonly Sector[,] _sectors;
        private readonly static Dictionary<bool, Brush> s_booleanValueColor = new()
        {
            [true] = new SolidColorBrush(Color.FromRgb(142, 255, 111)),
            [false] = new SolidColorBrush(Color.FromRgb(255, 111, 111))
        };


        public SectorsGenerator(VisualContainer space, int sectorSize)
            : base(space)
        {
            _sectorSize = Sector.Size = sectorSize;

            int sectorCountX = (int)Math.Floor(Space.Width / sectorSize),
                sectorCountY = (int)Math.Floor(Space.Height / sectorSize);

            _startPoint = new Point((Space.Width - sectorCountX * sectorSize) / 2,
                                    (Space.Height - sectorCountY * sectorSize) / 2);

            _sectors = new Sector[sectorCountY, sectorCountX];
        }

        public override void Generate()
        {
            double x = _startPoint.X, y = _startPoint.Y;

            DrawingVisual visual = new();
            DrawingContext context = visual.RenderOpen();

            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                int sectorPositionIndex = i % 2 == 0 ? 0 : 1;

                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    _sectors[i, j] = new Sector(sectorPositionIndex++ % 2 == 0);
 
                    context.DrawRectangle(null, new Pen(s_booleanValueColor[_sectors[i, j].SectorFillAbility], 1),
                                                new Rect(x, y, _sectorSize - 1, _sectorSize - 1));

                    x += _sectorSize;
                }

                x = _startPoint.X;
                y += _sectorSize;
            }

            Space.AddElement(visual);
            context.Close();
        }

        public Sector[,] GetSectors() => _sectors;
        public Point GetStartPoint() => _startPoint;
    }
}
