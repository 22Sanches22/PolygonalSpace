namespace PolygonalSpace.GenerationComponents
{
    internal readonly struct Sector
    {
        public static int Size { get; set; }
        public bool SectorFillAbility { get; }


        public Sector(bool sectorFillAbility) => SectorFillAbility = sectorFillAbility;
    }
}
