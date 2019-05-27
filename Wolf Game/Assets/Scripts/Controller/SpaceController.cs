using UnityEngine;
using System;

public class SpaceController : MonoBehaviour
{
    public GameObject SpaceView;
    public SpaceModel model;

    public void SetSpaceView(SpaceModel model, GameObject SpaceView, RandomSeeds seeds)
    {
        this.model = model;
        this.SpaceView = SpaceView;

        var doubled = model.GetDoubledCoords();

        Color color = new Color(0,0,0);

        var dist = model.DistCenter();

        var elevationPerlin = Mathf.PerlinNoise((seeds.elevation + doubled.row)*0.4f, (seeds.elevation + doubled.col/2)*0.4f);
        var temperaturePerlin = Mathf.PerlinNoise((seeds.baseTerrain + doubled.row)*0.2f, (seeds.baseTerrain + doubled.col/2)*0.2f);
        var moisturePerlin = Mathf.PerlinNoise((seeds.feature + doubled.row)*0.5f, (seeds.feature + doubled.col/2)*0.5f);

        var elevation = (1 + elevationPerlin - dist) / 2;
        var temperature = (1 + temperaturePerlin - 0) / 2;

        // SpaceView.GetComponentInChildren<TextMesh>().text = (Mathf.Round(elevationPerlin*100)).ToString();

        
        if(elevation < 0.1f)
        {
            // Ocean
            color = new Color(0, 0, 255);
        }
        else if (elevation < 0.8f)
        {
            // Flat
            if(temperature < 0.1f)
            {
                // Tundra
                color = new Color(200, 200, 200);
            }
            else if(temperature < 0.75)
            {
                // Plains?
                color = new Color(255, 0, 255);
            }
            else if(temperature < 0.9)
            {
                color = new Color(255, 255, 0);
            }
            else
            {
                color = new Color(0, 60, 0);
            }
        }
        else if (elevation < 0.9f)
        {
            // Hill
            color = new Color(0, 255, 255);
        }
        else //if (elevation < 1f)
        {
            // Mountain
            color = new Color(0, 0, 0);
        }

        SpaceView.GetComponent<Renderer>().materials[0].SetColor("_Color", color);
    }
}
