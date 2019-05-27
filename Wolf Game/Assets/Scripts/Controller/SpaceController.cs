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

        var elevation = (1 + Mathf.PerlinNoise(seeds.elevation + doubled.row, seeds.elevation + doubled.col) - model.DistCenter()) / 2;
        var temperature = Mathf.PerlinNoise(seeds.baseTerrain + doubled.row, seeds.baseTerrain + doubled.col);
        var moisture = Mathf.PerlinNoise(seeds.feature + doubled.row, seeds.feature + doubled.col);
        if(elevation < 0.3f)
        {
            color = new Color(0, 128, 255);
        }
        else if (elevation < 0.5f)
        {
        }
        else if (elevation < 0.6f)
        {
            color = new Color(96, 96, 96);
        }

        SpaceView.GetComponent<Renderer>().materials[0].SetColor("_Color", color);
    }
}
