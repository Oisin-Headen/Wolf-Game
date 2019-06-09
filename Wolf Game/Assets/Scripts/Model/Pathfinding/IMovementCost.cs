using System;using Pathfinding;

public interface IMovementCost{    int GetMovementCost(SpaceModel space);}public class OneCostMovement : IMovementCost
{    public int GetMovementCost(SpaceModel space)
    {
        return PathfindingDijkstras.ONE_SPACE;
    }
}public class OrdinaryMovementCost : IMovementCost
{
    public int GetMovementCost(SpaceModel space)
    {
        if (!space.Explored)
        {
            return PathfindingDijkstras.REST_OF_MOVEMENT;
        }
        if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water ||
           space.Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain)
        {
            // TODO Impassable
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