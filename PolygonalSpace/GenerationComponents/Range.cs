namespace PolygonalSpace.GenerationComponents
{
    internal readonly struct Range
    {
        public double Min { get; }
        public double Max { get; }

        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }
    }
}
