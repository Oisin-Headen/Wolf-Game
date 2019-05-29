using System;

public class SpaceModel
{
    public int cube_x, cube_y, cube_z;
    public SpaceController controller;
    private MapModel map;

    private DoubledCoords doubledCoords;
    private SpaceTerrain terrain;
    private SpaceModel[] adjacentSpaces;
    private PathfindingNode pathfindingNode;

    public SpaceModel(int row, int col, GameController gameController, MapModel map)
    {
        this.map = map;
        cube_x = (col - row) / 2;
        cube_z = row;
        cube_y = -cube_x-cube_z;
        doubledCoords = new DoubledCoords(cube_x, cube_y, cube_z);
        controller = gameController.AddSpace(this);
    }

    public DoubledCoords GetDoubledCoords()
    {
        return doubledCoords;
    }


    public SpaceModel[] GetAdjacentSpaces()
    {
        return adjacentSpaces;
    }
    public int GetMovementCost()
    {
        if(terrain.elevation == SpaceTerrain.SpaceElevation.Water ||
           terrain.elevation == SpaceTerrain.SpaceElevation.Mountain)
        {
            // TODO Impassable
        }
        if(terrain.elevation == SpaceTerrain.SpaceElevation.Hill ||
           terrain.feature == SpaceTerrain.SpaceFeature.Forest ||
           terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
        {
            return 2;
        }
        return 1;
    }
    public void SetAdjacentSpaces()
    {
        adjacentSpaces = new SpaceModel[6];
        adjacentSpaces[0] = map.GetNE(doubledCoords);
        adjacentSpaces[1] = map.GetE(doubledCoords);
        adjacentSpaces[2] = map.GetSE(doubledCoords);
        adjacentSpaces[3] = map.GetSW(doubledCoords);
        adjacentSpaces[4] = map.GetW(doubledCoords);
        adjacentSpaces[5] = map.GetNW(doubledCoords);
    }

    public PathfindingNode GetNode()
    {
        return pathfindingNode;
    }
    public void SetNode(PathfindingNode node)
    {
        pathfindingNode = node;
    }

    public void SetTerrain(SpaceTerrain terrain)
    {
        this.terrain = terrain;
    }

    public float DistCenter()
    {
        float distSideEdge, distTopBottomEdge;

        distSideEdge = Math.Abs(Math.Abs(Utilities.MAP_WIDTH/2-doubledCoords.col/2));
        distTopBottomEdge = Math.Abs(Math.Abs(Utilities.MAP_HEIGHT/2-doubledCoords.row));

        return Math.Max(distSideEdge/(Utilities.MAP_WIDTH/2), distTopBottomEdge/(Utilities.MAP_HEIGHT/2));
    }

    public float WorldTemp()
    {
        float distTopBottomEdge;

        distTopBottomEdge = Math.Abs(Utilities.MAP_HEIGHT/2-doubledCoords.row);

        // TODO Mountains are also cold

        return distTopBottomEdge/(Utilities.MAP_HEIGHT/2);
    }
}


public class DoubledCoords
{
    public int row, col;

    public DoubledCoords(int x, int y, int z)
    {
        col = 2 * x + z;
        row = z;
    }
}