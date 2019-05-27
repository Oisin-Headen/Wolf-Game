using UnityEngine;

public class MapModel
{
    SpaceModel[][] map;
    public MapModel(GameController gameController)
    {
        map = new SpaceModel[Utilities.MAP_HEIGHT][];
        for(int row = 0; row < Utilities.MAP_HEIGHT; ++row)
        {
            map[row] = new SpaceModel[Utilities.MAP_WIDTH * 2];
            
            int col = 0;
            if(row % 2 != 0)
            {
                ++col;
            }
            for(; col < Utilities.MAP_WIDTH * 2; col += 2)
            {
                map[row][col] = new SpaceModel(row, col, gameController);
            }
        }
    }
}


public enum SpaceElevation
{
    Water, Flat, Hill, Mountain
}
public enum SpaceBaseTerrain
{
    Grassland, Plain, Desert, Tundra

}
public enum SpaceFeature
{
    None, Forest, Deep_Forest
}

public class SpaceTerrain
{
    public SpaceElevation elevation;
    public SpaceBaseTerrain baseTerrain;
    public SpaceFeature feature;

    public SpaceTerrain(SpaceElevation elevation, SpaceBaseTerrain baseTerrain, SpaceFeature feature)
    {
        this.elevation = elevation;
        this.baseTerrain = baseTerrain;
        this.feature = feature;
    }
}
