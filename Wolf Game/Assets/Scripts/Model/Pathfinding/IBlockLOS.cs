using Model;

namespace Pathfinding
{
    public interface IBlockLOS
    {
        bool BlocksLOS(SpaceTerrain.SpaceElevation elevation, SpaceModel space);
    }

    // Normal line of sight method
    public class NormalLOS : IBlockLOS
    {
        public bool BlocksLOS(SpaceTerrain.SpaceElevation elevation, SpaceModel space)
        {
            // Mountains and Icebergs Block LOS
            bool blocked = space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain ||
                space.Terrain.feature == SpaceTerrain.SpaceFeature.Iceberg;

            // If on Flat ground, all forests block LOS
            if (elevation == SpaceTerrain.SpaceElevation.Flat)
            {
                blocked = space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest ||
                space.Terrain.feature == SpaceTerrain.SpaceFeature.Forest;
            }
            // If on a Hill, only Forested Hills block LOS
            else if (elevation == SpaceTerrain.SpaceElevation.Hill)
            {
                bool forested = space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest ||
                    space.Terrain.feature == SpaceTerrain.SpaceFeature.Forest;
                blocked = forested && space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill;
            }

            // If Not on a hill or mountain, Hills block LOS
            if (!(elevation != SpaceTerrain.SpaceElevation.Hill ||
                elevation != SpaceTerrain.SpaceElevation.Mountain))
            { blocked |= space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill; }

            return blocked;
        }
    }

    // Ignore forests when determining LOS
    public class IgnoreForestLOS : IBlockLOS
    {
        public bool BlocksLOS(SpaceTerrain.SpaceElevation elevation, SpaceModel space)
        {
            bool blocked = space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain ||
                        space.Terrain.feature == SpaceTerrain.SpaceFeature.Iceberg;

            if (!(elevation == SpaceTerrain.SpaceElevation.Hill ||
                elevation == SpaceTerrain.SpaceElevation.Mountain))
            { blocked |= space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill; }
            return blocked;
        }
    }
}