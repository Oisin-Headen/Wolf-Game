using System;

public class SpaceModel
{
    public int cube_x, cube_y, cube_z;
    public SpaceController controller;

    public SpaceModel(int row, int col, GameController gameController)
    {
        cube_x = (col - row) / 2;
        cube_z = row;
        cube_y = -cube_x-cube_z;
        controller = gameController.AddSpace(this);
    }

    public DoubledCoords GetDoubledCoords()
    {
        return new DoubledCoords(cube_x, cube_y, cube_z);
    }

    public float DistCenter()
    {
        const int centerX = Utilities.MAP_WIDTH/2, centerY=Utilities.MAP_HEIGHT/2;

        float highest = Math.Max(centerX, centerY);

        const int center_x = (centerY - centerX) / 2;
        const int center_z = centerX;
        const int center_y = -center_x-center_z;

        int spacesCenter = Math.Max(Math.Max(Math.Abs(cube_x - centerX), Math.Abs(cube_y - center_y)), Math.Abs(cube_z - center_z));
        return spacesCenter/highest;
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