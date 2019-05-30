using System;

public class SpaceModel
{
    public SpaceController controller;
    private readonly GameController gameController;
    private readonly MapModel map;

    private readonly DoubledCoords doubledCoords;
    private readonly CubeCoords cubeCoords;

    private SpaceTerrain terrain;
    private SpaceModel[] adjacentSpaces;
    private PathfindingNode pathfindingNode;

    public SpaceModel(int row, int col, GameController gameController, MapModel map)
    {
        this.map = map;
        float cube_x = (col - row) / 2;
        float cube_z = row;
        float cube_y = -cube_x-cube_z;
        cubeCoords = new CubeCoords(cube_x, cube_y, cube_z);
        doubledCoords = new DoubledCoords(row, col);
        this.gameController = gameController;
        controller = gameController.AddSpace(this);
    }

    public DoubledCoords GetDoubledCoords()
    {
        return doubledCoords;
    }

    public string GetDescription()
    {
        string desc = "";
        switch(terrain.elevation)
        {
            case SpaceTerrain.SpaceElevation.Water:
                desc = "Ocean";
                if(terrain.feature == SpaceTerrain.SpaceFeature.Iceberg)
                { desc = "Iceberg"; }
                break;
            case SpaceTerrain.SpaceElevation.Mountain:
                desc = "Mountain";
                if (terrain.feature == SpaceTerrain.SpaceFeature.Frosted)
                { desc = "Frosted Mountain"; }
                break;
            default:
                string hill = "";
                if(terrain.elevation == SpaceTerrain.SpaceElevation.Hill)
                { hill = " Hill"; }
                string baseTerrain = "Error", feature = "";
                switch(terrain.baseTerrain)
                {
                    case SpaceTerrain.SpaceBaseTerrain.Desert:
                        baseTerrain = "Desert";
                        break;
                    case SpaceTerrain.SpaceBaseTerrain.Snow:
                        baseTerrain = "Snow";
                        break;
                    case SpaceTerrain.SpaceBaseTerrain.Tundra:
                        baseTerrain = "Tundra";
                        break;
                    case SpaceTerrain.SpaceBaseTerrain.Plain:
                        baseTerrain = "Plains";
                        break;
                    case SpaceTerrain.SpaceBaseTerrain.Grassland:
                        baseTerrain = "Grassland";
                        break;
                }
                switch (terrain.feature)
                {
                    case SpaceTerrain.SpaceFeature.Forest:
                        feature = " Forest";
                        break;
                    case SpaceTerrain.SpaceFeature.Deep_Forest:
                        feature = " Deep Forest";
                        break;
                    case SpaceTerrain.SpaceFeature.None:
                        break;
                    default:
                        feature = " Error";
                        break;
                }
                desc = baseTerrain + feature + hill;
                break;
        }

        return desc;
    }

    public CubeCoords GetCubeCoords()
    {
        return cubeCoords;
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

    public SpaceTerrain GenerateTerrain()
    {
        terrain = Utilities.GetTerrainForSpace(this, gameController.seeds);
        return terrain;
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

    public bool Occupied()
    {
        // TODO Is there a Unit here?
        throw new NotImplementedException();
    }
}