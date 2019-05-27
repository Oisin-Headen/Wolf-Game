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

        var elevationPerlin = Mathf.PerlinNoise((seeds.elevation + doubled.row)*0.2f, (seeds.elevation + doubled.col/2f)*0.2f);
        var temperaturePerlin = Mathf.PerlinNoise((seeds.baseTerrain + doubled.row)*0.4f, (seeds.baseTerrain + doubled.col/2f)*0.4f);
        var moisturePerlin = Mathf.PerlinNoise((seeds.feature + doubled.row)*0.5f, (seeds.feature + doubled.col/2f)*0.5f);

        var elevation =(1 + elevationPerlin - dist) / 2; //  elevationPerlin - dist;
        // var elevation = dist + elevationPerlin * (0.5 - dist);

        var temperature = (1 + temperaturePerlin - 0) / 2;

        // SpaceView.GetComponentInChildren<TextMesh>().text = (Mathf.Round(elevationPerlin*100)).ToString();

        
        if(elevation < 0.4f)
        {
            // Ocean
            color = new Color(0, 0, 255);
        }
        else if (elevation < 0.7f)
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
                color = new Color(1f, 1f, 1f);
            }
            else if(temperature < 0.9)
            {
                color = new Color(0, 1, 0);
            }
            else
            {
                color = new Color(0, 0.5f, 0);
            }
        }
        else if (elevation < 0.8f)
        {
            // Hill
            color = new Color(0.7f, 0.7f, 0.7f);
        }
        else //if (elevation < 1f)
        {
            // Mountain
            color = new Color(0, 0, 0);
        }

        SpaceView.GetComponent<Renderer>().materials[0].SetColor("_Color", color);
    }
}
