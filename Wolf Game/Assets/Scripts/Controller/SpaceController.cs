using UnityEngine;
using System;

public class SpaceController : MonoBehaviour
{
    private GameObject SpaceView;
    public SpaceModel model;

    public Sprite MountainFrost;
    public Sprite Mountain;
    public Sprite Desert;
    public Sprite DesertHills;
    public Sprite Grass;
    public Sprite GrassDeepForest;
    public Sprite GrassForest;
    public Sprite GrassHill;
    public Sprite GrassHillDeepForest;
    public Sprite GrassHillForest;
    public Sprite Plains;
    public Sprite PlainsDeepForest;
    public Sprite PlainsForest;
    public Sprite PlainsHill;
    public Sprite PlainsHillDeepForest;
    public Sprite PlainsHillForest;
    public Sprite Snow;
    public Sprite SnowHills;
    public Sprite Tundra;
    public Sprite TundraForest;
    public Sprite TundraHill;
    public Sprite TundraHillForest;
    public Sprite Water;
    public Sprite WaterIceberg;


    public void SetSpaceView(SpaceModel model, GameObject SpaceView, RandomSeeds seeds)
    {
        this.model = model;
        this.SpaceView = SpaceView;

        var doubled = model.GetDoubledCoords();

        Color color = new Color(0,0,0);

        var distEdge = model.DistCenter();
        var worldTemp = model.WorldTemp();

        var elevationPerlin = Mathf.PerlinNoise((seeds.elevation + doubled.row)*0.2f, (seeds.elevation + doubled.col/2f)*0.2f);
        var temperaturePerlin = Mathf.PerlinNoise((seeds.baseTerrain + doubled.row)*0.4f, (seeds.baseTerrain + doubled.col/2f)*0.4f);
        var moisturePerlin = Mathf.PerlinNoise((seeds.feature + doubled.row)*0.5f, (seeds.feature + doubled.col/2f)*0.5f);

        var elevation =(1 + elevationPerlin - distEdge) / 2; //  elevationPerlin - dist;
        // var elevation = dist + elevationPerlin * (0.5 - dist);

        var temperature = (1 + temperaturePerlin - worldTemp) / 2;

        // SpaceView.GetComponentInChildren<TextMesh>().text = (Mathf.Round(elevationPerlin*100)).ToString();

        // Ocean --------------------------------------------------------------------------
        if(elevation < 0.4f)
        {
            if(temperature < 0.1)
            {
                // Iceberg
                SpaceView.GetComponent<SpriteRenderer>().sprite = WaterIceberg;
            }
            else
            {
                // Ocean
                SpaceView.GetComponent<SpriteRenderer>().sprite = Water;
            }
        }
        // Flat --------------------------------------------------------------------------
        else if (elevation < 0.7f)
        {
            // Flat
            if(temperature < 0.1f)
            {
                // Cold
                if(moisturePerlin < 0.5f)
                {
                    // Tundra
                    SpaceView.GetComponent<SpriteRenderer>().sprite = Tundra;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = Snow;
                }
            }
            else if(temperature < 0.3)
            {
                // Cool
                if(moisturePerlin < 0.1f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = Desert;
                }
                else if(moisturePerlin < 0.4f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = Plains;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = TundraForest;
                }
            }
            else if(temperature < 0.9)
            {
                // Warm
                if(moisturePerlin < 0.5f)
                {
                    // Plains
                    SpaceView.GetComponent<SpriteRenderer>().sprite = Plains;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = GrassForest;
                }
            }
            else
            {
                // Hot
                if(moisturePerlin < 0.1f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = Desert;
                }
                else if(moisturePerlin < 0.4f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = Grass;
                }
                else if(moisturePerlin < 0.8f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = GrassForest;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = GrassDeepForest;
                }
            }
        }
        // Hill --------------------------------------------------------------------------
        else if (elevation < 0.8f)
        {
            if(temperature < 0.1f)
            {
                // Cold
                if(moisturePerlin < 0.5f)
                {
                    // Tundra
                    SpaceView.GetComponent<SpriteRenderer>().sprite = TundraHill;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = SnowHills;
                }
            }
            else if(temperature < 0.3)
            {
                // Cool
                if(moisturePerlin < 0.1f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = DesertHills;
                }
                else if(moisturePerlin < 0.4f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = PlainsHill;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = TundraHillForest;
                }
            }
            else if(temperature < 0.9)
            {
                // Warm
                if(moisturePerlin < 0.5f)
                {
                    // Plains
                    SpaceView.GetComponent<SpriteRenderer>().sprite = PlainsHill;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = GrassHillForest;
                }
            }
            else
            {
                // Hot
                if(moisturePerlin < 0.1f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = DesertHills;
                }
                else if(moisturePerlin < 0.4f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = GrassHill;
                }
                else if(moisturePerlin < 0.8f)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = GrassHillForest;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = GrassHillDeepForest;
                }
            }
        }
        // Mountain --------------------------------------------------------------------------
        else //if (elevation < 1f)
        {
            if(temperature < 0.1f)
            {
                SpaceView.GetComponent<SpriteRenderer>().sprite = MountainFrost;
            }
            else
            {
                SpaceView.GetComponent<SpriteRenderer>().sprite = Mountain;
            }
        }

        SpaceView.GetComponent<Renderer>().materials[0].SetColor("_Color", color);
    }
}
