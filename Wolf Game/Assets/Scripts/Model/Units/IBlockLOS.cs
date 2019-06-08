public interface IBlockLOS{    bool BlocksLOS(SpaceTerrain.SpaceElevation elevation, SpaceModel space);}// Normal line of sight methodpublic class NormalLOS : IBlockLOS
{
    public bool BlocksLOS(SpaceTerrain.SpaceElevation elevation, SpaceModel space)
    {        bool blocked = space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain ||            space.Terrain.feature == SpaceTerrain.SpaceFeature.Iceberg ||            space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest ||            space.Terrain.feature == SpaceTerrain.SpaceFeature.Forest;        if (!(elevation != SpaceTerrain.SpaceElevation.Hill ||            elevation != SpaceTerrain.SpaceElevation.Mountain))
        { blocked |= space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill; }
        return blocked;
    }
}// Ignore forests when determining LOSpublic class IgnoreForestLOS : IBlockLOS
{
    public bool BlocksLOS(SpaceTerrain.SpaceElevation elevation, SpaceModel space)
     {
        bool blocked = space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain ||
                    space.Terrain.feature == SpaceTerrain.SpaceFeature.Iceberg;        if (!(elevation == SpaceTerrain.SpaceElevation.Hill ||            elevation == SpaceTerrain.SpaceElevation.Mountain))        { blocked |= space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill; }        return blocked;
    }
}