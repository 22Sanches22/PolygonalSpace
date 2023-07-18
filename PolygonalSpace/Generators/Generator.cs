using PolygonalSpace.GenerationComponents;

namespace PolygonalSpace.Generators
{
    internal abstract class Generator
    {
        protected readonly VisualContainer Space;


        protected Generator(VisualContainer space) => Space = space;

        public abstract void Generate();
        public void Clear() => Space.Clear();
    }
}
