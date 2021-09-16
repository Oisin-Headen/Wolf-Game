using System;
using Model;

namespace Pathfinding
{
    public interface IMovementCost
    {
        int GetMovementCost(SpaceModel space);
    }
    // For Finding absolute distance.
    public class OneCostMovement : IMovementCost
    {
        public int GetMovementCost(SpaceModel space)
        {
            return PathfindingDijkstras.ONE_SPACE;
        }
    }

    // The Default way to determine cost.
    public class OrdinaryMovementCost : IMovementCost
    {
        public int GetMovementCost(SpaceModel space)
        {
            if (!space.Explored)
            {
                return 2 * PathfindingDijkstras.ONE_SPACE;
            }
            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water ||
               space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain)
            {
                // Impassable
                return -1;
            }
            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill ||
               space.Terrain.feature == SpaceTerrain.SpaceFeature.Forest ||
               space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
            {
                return 2 * PathfindingDijkstras.ONE_SPACE;
            }
            return 1 * PathfindingDijkstras.ONE_SPACE;
        }
    }

    // TODO: I'm pretty sure this is totally redundant, anything that can go through Deep forest at half
    // can go through forest at one as well.
    //public class DeepForestAtHalfMovementCost : IMovementCost
    //{
    //    public int GetMovementCost(SpaceModel space)
    //    {
    //        if (!space.Explored)
    //        {
    //            return 2 * PathfindingDijkstras.ONE_SPACE;
    //        }
    //        if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water ||
    //            space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain)
    //        {
    //            // Impassable
    //            return -1;
    //        }
    //        if (space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
    //        {
    //            return PathfindingDijkstras.HALF_SPACE;
    //        }
    //        if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill ||
    //            space.Terrain.feature == SpaceTerrain.SpaceFeature.Forest)
    //        {
    //            return 2 * PathfindingDijkstras.ONE_SPACE;
    //        }
    //        return 1 * PathfindingDijkstras.ONE_SPACE;
    //    }
    //}


    public class DeepForestAtHalfMovementCostForestAtOne : IMovementCost
    {
        public int GetMovementCost(SpaceModel space)
        {
            if (!space.Explored)
            {
                return 2 * PathfindingDijkstras.ONE_SPACE;
            }
            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water ||
                space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain)
            {
                // Impassable
                return -1;
            }
            if (space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
            {
                return PathfindingDijkstras.HALF_SPACE;
            }
            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill)
            {
                return 2 * PathfindingDijkstras.ONE_SPACE;
            }
            return 1 * PathfindingDijkstras.ONE_SPACE;
        }
    }

    public class OneCostWaterMovement : IMovementCost
    {
        public int GetMovementCost(SpaceModel space)
        {
            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water)
            {
                return PathfindingDijkstras.ONE_SPACE;
            }
            return -1;
        }
    }
}