using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SpaceController : MonoBehaviour
{
    private GameObject SpaceView;
    private GameController gameController;
    public SpaceModel model;
    
    

    
    // Setup this Controller
    public void SetSpaceView(SpaceModel model, GameObject SpaceView, GameController gameController)
    {
        this.gameController = gameController;
        this.model = model;
        this.SpaceView = SpaceView;

        var terrain = model.GenerateTerrain();


        // SpaceView.GetComponent<SpriteRenderer>().material.color = terrain.color;

        // Set the View to the Correct Tile Type
        SetTerrain(terrain);
    
    }

    public Vector2 GetPosition()
    {
        return SpaceView.transform.position;
    }

    // If the user mouses over this hex, display properties
    private void OnMouseOver()
    {
        string text = model.GetDescription();
        gameController.mouseOverText.text = text;
    }



    // This Method Sets the hex view to the correct terrain
    public void SetTerrain(SpaceTerrain terrain)
    {
        switch (terrain.elevation)
        {
            // An Ocean Tile
            case SpaceTerrain.SpaceElevation.Water:
                if (terrain.feature == SpaceTerrain.SpaceFeature.Iceberg)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.WaterIceberg;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Water;
                }
                break;

            // A Flat Tile
            case SpaceTerrain.SpaceElevation.Flat:
                switch (terrain.baseTerrain)
                {
                    case SpaceTerrain.SpaceBaseTerrain.Desert:
                        SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Desert;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Grassland:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassDeepForest;
                        }
                        else
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Grass;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Plain:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsDeepForest;
                        }
                        else
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Plains;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Snow:
                        SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Snow;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Tundra:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraForest;
                        }
                        else
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Tundra;
                        }
                        break;

                    default:
                        // ERROR
                        break;
                }
                break;

            // A Hill Tile
            case SpaceTerrain.SpaceElevation.Hill:
                switch (terrain.baseTerrain)
                {
                    case SpaceTerrain.SpaceBaseTerrain.Desert:
                        SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.DesertHills;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Grassland:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHillForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHillDeepForest;
                        }
                        else
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHill;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Plain:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHillForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHillDeepForest;
                        }
                        else
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHill;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Snow:
                        SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.SnowHills;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Tundra:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraHillForest;
                        }
                        else
                        {
                            SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraHill;
                        }
                        break;

                    default:
                        // ERROR
                        break;
                }
                break;

            // A Mountain Tile
            case SpaceTerrain.SpaceElevation.Mountain:
                if (terrain.feature == SpaceTerrain.SpaceFeature.Frosted)
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.MountainFrost;
                }
                else
                {
                    SpaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Mountain;
                }
                break;
        }
    }
}
