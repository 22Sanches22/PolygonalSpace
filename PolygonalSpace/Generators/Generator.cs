using PolygonalSpace.GenerationComponents;

namespace PolygonalSpace.Generators
{
    internal abstract class Generator
    {
        protected readonly VisualContainer _space;


        protected Generator(VisualContainer space) => _space = space;

        public abstract void Generate();
        public void Clear() => _space.Clear();
    }
}
