using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SpaceController : MonoBehaviour
{
    private GameObject spaceView;
    private GameController gameController;
    private SpaceModel spaceModel;

    private GameObject tileSelectorPrefab;
    private GameObject currentTileSelector;

    private bool attackable, moveable;
    
    // Setup this Controller
    public void Setup(SpaceModel model, GameObject spaceView, GameController gameController)
    {
        this.gameController = gameController;
        this.spaceModel = model;
        this.spaceView = spaceView;

        tileSelectorPrefab = gameController.tileSelectorPrefab;
        currentTileSelector = null;

        var terrain = model.GenerateTerrain();
        SetTerrain(terrain);

        attackable = false;
        moveable = false;
    }

    public Vector2 GetPosition()
    {
        return spaceView.transform.position;
    }

    // If the user mouses over this hex, display properties
    private void OnMouseOver()
    {
        //string text = spaceModel.GetDescription();
        //gameController.mouseOverText.text = text;


        // if the Space is attackable or moveable, then change the color of the tile selector
        if(moveable)
        {
            CreateSelectorAndSetColor(gameController.assets.MoveableHighlightedColor);
        }
        else if(attackable)
        {
            CreateSelectorAndSetColor(gameController.assets.AttackableHighlightedColor);
        }
    }

    private void OnMouseExit()
    {
        if (moveable)
        {
            CreateSelectorAndSetColor(gameController.assets.MoveableColor);
        }
        else if (attackable)
        {
            CreateSelectorAndSetColor(gameController.assets.AttackableColor);
        }
    }

    private void OnMouseUpAsButton()
    {
        spaceModel.Clicked();
    }

// *** A List of methods that change the selectable/selected look of this tile *********************

    // When a tile is clicked on, and is the selected tile.
    public void SetSelected()
    {
        CreateSelectorAndSetColor(gameController.assets.SelectedColor);
        gameController.cameraController.SetPosition(GetPosition());
    }

    // When a tile can be moved to.
    public void SetMoveable()
    {
        CreateSelectorAndSetColor(gameController.assets.MoveableColor);
        moveable = true;
    }

    // When a tile can be Attacked.
    public void SetAttackable()
    {
        CreateSelectorAndSetColor(gameController.assets.AttackableColor);
        attackable = true;
    }

    private void CreateSelectorAndSetColor(Color color)
    {
        if (currentTileSelector == null)
        {
            currentTileSelector = Instantiate(tileSelectorPrefab, spaceView.transform.position,
                Quaternion.identity, spaceView.transform);
        }

        currentTileSelector.GetComponent<SpriteRenderer>().material.color = color;
    }

    // Tile is no longer selected.
    public void Deselect()
    {
        if (currentTileSelector != null)
        {
            Destroy(currentTileSelector);
            currentTileSelector = null;
        }
        moveable = false;
        attackable = false;
    }

// *** End Selectables *****************************************************************************

    public void Show()
    {
        spaceView.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }
    public void Hide()
    {
        spaceView.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
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
                    spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.WaterIceberg;
                }
                else
                {
                    spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Water;
                }
                break;

            // A Flat Tile
            case SpaceTerrain.SpaceElevation.Flat:
                switch (terrain.baseTerrain)
                {
                    case SpaceTerrain.SpaceBaseTerrain.Desert:
                        spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Desert;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Grassland:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassDeepForest;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Grass;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Plain:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsDeepForest;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Plains;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Snow:
                        spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Snow;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Tundra:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraForest;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Tundra;
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
                        spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.DesertHills;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Grassland:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHillForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHillDeepForest;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHill;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Plain:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHillForest;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHillDeepForest;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHill;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Snow:
                        spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.SnowHills;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Tundra:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraHillForest;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraHill;
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
                    spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.MountainFrost;
                }
                else
                {
                    spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Mountain;
                }
                break;
        }
    }


    // todo testing methods
    public void SetText(string v)
    {
        GetComponentInChildren<TextMesh>().text = v;
    }
}
