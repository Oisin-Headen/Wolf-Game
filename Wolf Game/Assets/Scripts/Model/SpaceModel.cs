using System;

public class SpaceModel
{
    public int cube_x, cube_y, cube_z;
    public SpaceController controller;

    private DoubledCoords doubledCoords;

    public SpaceModel(int row, int col, GameController gameController)
    {
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

        distTopBottomEdge = Math.Abs(Math.Abs(Utilities.MAP_HEIGHT/2-doubledCoords.row));

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