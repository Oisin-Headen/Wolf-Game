using System;
using Model;
using UnityEngine;

public static class Utilities
{
    // Huge Size: 128*80
    public const int MAP_WIDTH = 128;
    public const int MAP_HEIGHT = 80;
    public const float HEX_SIZE = 0.643f;

    public const int CAMERA_SPEED = 10;
    public const int CAMERA_ZOOM_SPEED = 15;
    // Usually 5
    public const float MAX_CAMERA_SIZE = 7.0f;
    public const float MIN_CAMERA_SIZE = 2.0f;

    public const int MIN_CAMERA_X = 0;
    public const int MIN_CAMERA_Y = 0;
    public const float MAX_CAMERA_X = HEX_SIZE * 1.73205f * MAP_WIDTH;

    public const float MAX_CAMERA_Y = HEX_SIZE * 3 / 2f * MAP_HEIGHT;

    public static SpaceTerrain GetTerrainForSpace(SpaceModel space, RandomSeeds seeds)
    {
        DoubledCoords coords = space.GetDoubledCoords();

        var distEdge = space.DistCenter();
        var worldTemp = space.WorldTemp();

        var elevationPerlin = Mathf.PerlinNoise((seeds.elevation + coords.row) * 0.2f, (seeds.elevation + coords.col / 2f) * 0.2f);
        var temperaturePerlin = Mathf.PerlinNoise((seeds.temperature + coords.row) * 0.5f, (seeds.temperature + coords.col / 2f) * 0.5f);
        var moisturePerlin = Mathf.PerlinNoise((seeds.moisture + coords.row) * 0.3f, (seeds.moisture + coords.col / 2f) * 0.3f);

        var elevation = (1 + elevationPerlin - distEdge) / 2;
        var temperature = (0.75f + temperaturePerlin * 0.25f - worldTemp * 0.75f);

        // float randTest = temperature;
        // var color = new Color(randTest,randTest,randTest);

        SpaceTerrain.SpaceElevation spaceElevation = SpaceTerrain.SpaceElevation.Flat;
        SpaceTerrain.SpaceBaseTerrain spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.None;
        SpaceTerrain.SpaceFeature spaceFeature = SpaceTerrain.SpaceFeature.None;

        if (elevation < 0.4f)
        {
            // This is an Ocean Tile
            spaceElevation = SpaceTerrain.SpaceElevation.Water;
            if (temperature < 0.2)
            {
                // Iceberg
                spaceFeature = SpaceTerrain.SpaceFeature.Iceberg;
            }
        }
        else if (elevation >= 0.8f)
        {
            // Mountian Tile
            spaceElevation = SpaceTerrain.SpaceElevation.Mountain;
            if (temperature < 0.2)
            {
                // Frosty Mountian
                spaceFeature = SpaceTerrain.SpaceFeature.Frosted;
            }
        }
        else
        {
            if (elevation >= 0.7f)
            {
                // A Hill in Addition to Below
                spaceElevation = SpaceTerrain.SpaceElevation.Hill;
            }
            if (temperature < 0.3f)
            {
                // Cold
                if (moisturePerlin < 0.5f)
                {
                    // Tundra
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Tundra;
                }
                else
                {
                    // Snow
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Snow;
                }
            }
            else if (temperature < 0.4)
            {
                // Cool
                if (moisturePerlin < 0.4f)
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Tundra;
                }
                else if (moisturePerlin < 0.6f)
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Plain;
                }
                else
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Tundra;
                    spaceFeature = SpaceTerrain.SpaceFeature.Forest;
                }
            }
            else if (temperature < 0.8)
            {
                // Warm
                if (moisturePerlin < 0.4f)
                {
                    // Plains
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Plain;
                }
                else if (moisturePerlin < 0.5f)
                {
                    // Plains forest
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Plain;
                    spaceFeature = SpaceTerrain.SpaceFeature.Forest;
                }
                else if (moisturePerlin < 0.6f)
                {
                    // grass
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Grassland;
                }
                else
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Grassland;
                    spaceFeature = SpaceTerrain.SpaceFeature.Forest;
                }
            }
            else
            {
                // Hot
                if (moisturePerlin < 0.35f)
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Desert;
                }
                else if (moisturePerlin < 0.6f)
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Grassland;
                }
                else if (moisturePerlin < 0.8f)
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Grassland;
                    spaceFeature = SpaceTerrain.SpaceFeature.Forest;
                }
                else
                {
                    spaceBaseTerrain = SpaceTerrain.SpaceBaseTerrain.Grassland;
                    spaceFeature = SpaceTerrain.SpaceFeature.Deep_Forest;
                }
            }
        }

        return new SpaceTerrain(spaceElevation, spaceBaseTerrain, spaceFeature);
    }
}



public class DoubledCoords
{
    public int row, col;

    public DoubledCoords(int row, int col)
    {

        this.col = col;
        this.row = row;
    }
}

public class CubeCoords
{
    public float x, y, z;

    public CubeCoords(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}


public class RandomSeeds
{
    public float elevation, temperature, moisture;
    public RandomSeeds()
    {
        System.Random random = new System.Random();
        elevation = random.Next(1000);
        temperature = random.Next(1000);
        moisture = random.Next(1000);
    }
}

public class SpaceTerrain
{
    public enum SpaceElevation
    {
        Water, Flat, Hill, Mountain
    }
    public enum SpaceBaseTerrain
    {
        Grassland, Plain, Desert, Tundra, Snow, None

    }
    public enum SpaceFeature
    {
        None, Forest, Deep_Forest, Iceberg, Frosted
    }

    public SpaceElevation elevation;
    public SpaceBaseTerrain baseTerrain;
    public SpaceFeature feature;

    //Map generation debug purposes
    // public Color color;

    public SpaceTerrain(SpaceElevation elevation, SpaceBaseTerrain baseTerrain, SpaceFeature feature)
    {
        this.elevation = elevation;
        this.baseTerrain = baseTerrain;
        this.feature = feature;
    }

    // public SpaceTerrain(SpaceElevation elevation, SpaceBaseTerrain baseTerrain, SpaceFeature feature, Color color)
    // {
    //     this.elevation = elevation;
    //     this.baseTerrain = baseTerrain;
    //     this.feature = feature;
    //     this.color = color;
    // }
}

public enum Player
{
    Wolves, Alliance //, Necromancer, Goblins, Artificer, Wizards
}