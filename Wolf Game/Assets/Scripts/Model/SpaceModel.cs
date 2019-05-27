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
        const int centerX = Utilities.MAP_WIDTH, centerY=Utilities.MAP_HEIGHT/2;

        float highest = Math.Max(centerX, centerY);

        // const int center_x = (centerY - centerX) / 2;
        // const int center_z = centerX;
        // const int center_y = -center_x-center_z;

        // int spacesCenter = Math.Max(Math.Max(Math.Abs(cube_x - centerX), Math.Abs(cube_y - center_y)), Math.Abs(cube_z - center_z));\\

        var dx = Math.Abs(doubledCoords.col - centerX);
        var dy = Math.Abs(doubledCoords.row - centerY);

        float distance = dy + Math.Max(0, (dx-dy)/2f);


        float distSideEdge, distTopEdge;

        distSideEdge = Math.Abs(Math.Abs(Utilities.MAP_WIDTH/2-doubledCoords.col/2));
        distTopEdge = Math.Abs(Math.Abs(Utilities.MAP_HEIGHT/2-doubledCoords.row));

        // if(distSideEdge > Utilities.MAP_WIDTH/2 - 4)
        // {
        //     return 10f;
        // }
        // if(distTopEdge > Utilities.MAP_HEIGHT/2 - 4)
        // {
        //     return 10f;
        // }

        return Math.Max(distSideEdge/(Utilities.MAP_WIDTH/2), distTopEdge/(Utilities.MAP_HEIGHT/2));

        // return 0;
        // return distance/80f;
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