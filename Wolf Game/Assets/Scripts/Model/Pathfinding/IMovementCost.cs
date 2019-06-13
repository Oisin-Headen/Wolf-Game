using System;using Model;
using Pathfinding;
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
    {        private int unexploredCost;        public OrdinaryMovementCost(int unexploredCost)
        {
            this.unexploredCost = unexploredCost;
        }
        public int GetMovementCost(SpaceModel space)
        {
            if (!space.Explored)
            {
                return unexploredCost * PathfindingDijkstras.ONE_SPACE;
            }
            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water ||
               space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain)
            {                // Impassable
                return -1;
            }
            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill ||
               space.Terrain.feature == SpaceTerrain.SpaceFeature.Forest ||
               space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
            {
                return 2 * PathfindingDijkstras.ONE_SPACE;
            }
            return 1 * PathfindingDijkstras.ONE_SPACE;
        }    }    public class DeepForestAtHalfMovementCost : IMovementCost
    {        private int unexploredCost;        public DeepForestAtHalfMovementCost(int unexploredCost)        {            this.unexploredCost = unexploredCost;        }
        public int GetMovementCost(SpaceModel space)        {
            if (!space.Explored)            {
                return unexploredCost * PathfindingDijkstras.ONE_SPACE;
            }            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water ||               space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain)            {                // Impassable                return -1;            }            if(space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
            {
                return PathfindingDijkstras.HALF_SPACE;
            }            if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Hill ||               space.Terrain.feature == SpaceTerrain.SpaceFeature.Forest)            {                return 2 * PathfindingDijkstras.ONE_SPACE;            }            return 1 * PathfindingDijkstras.ONE_SPACE;
        }
    }
}