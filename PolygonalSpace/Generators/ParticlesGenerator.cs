using PolygonalSpace.GenerationComponents;
using System;
using System.Windows;
using System.Windows.Media;

namespace PolygonalSpace.Generators
{
    internal class ParticlesGenerator : Generator
    {
        private readonly int _numberParticles;
        private readonly Range _radiusRange;     
        private readonly Color _color;


        public ParticlesGenerator(VisualContainer space, int numberParticles, Range radiusRange, Color color)
            : base(space)
        {
            (_numberParticles, _radiusRange, _color) = (numberParticles, radiusRange, color);
        }

        public override void Generate()
        {
            Random random = new();

            DrawingVisual visual = new();
            DrawingContext context = visual.RenderOpen();

            for (int i = 0; i < _numberParticles; i++)
            {
                double particleRadius = (_radiusRange.Max - _radiusRange.Min) * random.NextDouble() + _radiusRange.Min;
                int particleDiameter = (int)Math.Round(particleRadius * 2);

                double zoomPercentage = 100d / (_radiusRange.Max - _radiusRange.Min) * (particleRadius - _radiusRange.Min);
                Color color = Color.FromRgb(GetValueFromPercentage(_color.R, zoomPercentage),
                                            GetValueFromPercentage(_color.G, zoomPercentage),
                                            GetValueFromPercentage(_color.B, zoomPercentage));

                Point positioningPoint = new(random.Next(particleDiameter, (int)(Space.Width - particleDiameter)),
                                             random.Next(particleDiameter, (int)(Space.Height - particleDiameter)));

                context.DrawEllipse(new SolidColorBrush(color), null, positioningPoint, particleRadius, particleRadius);
            }

            Space.AddElement(visual);
            context.Close();
        }

        private byte GetValueFromPercentage(byte maxValue, double percent) =>
            (byte)Math.Round(maxValue / 100d * percent);
    }
}
