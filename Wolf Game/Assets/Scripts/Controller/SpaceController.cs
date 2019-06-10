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
    private SpriteRenderer minimapDisplay;

    private bool attackable, moveable;

    // Setup this Controller
    public void Setup(SpaceModel model, GameObject spaceView, GameController gameController)
    {
        minimapDisplay = spaceView.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        this.gameController = gameController;
        this.spaceModel = model;
        this.spaceView = spaceView;

        tileSelectorPrefab = gameController.tileSelectorPrefab;
        currentTileSelector = null;

        var terrain = model.GenerateTerrain();
        SetTerrain(terrain);

        attackable = false;
        moveable = false;
        Hide();
    }

    public Vector2 GetPosition()
    {
        return spaceView.transform.position;
    }

    // If the user mouses over this hex, display properties
    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            string text = spaceModel.GetDescription();
            gameController.mouseOverText.text = text;


            // if the Space is attackable or moveable, then change the color of the tile selector
            if (moveable)
            {
                CreateSelectorAndSetColor(gameController.assets.MoveableHighlightedColor);
            }
            else if (attackable)
            {
                CreateSelectorAndSetColor(gameController.assets.AttackableHighlightedColor);
            }
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
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            spaceModel.Clicked();
        }
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


        var childColor = minimapDisplay.color;
        childColor.a = 1;
        minimapDisplay.color = childColor;
    }
    public void Hide()
    {
        spaceView.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        var childColor = minimapDisplay.color;
        childColor.a = 0;
        minimapDisplay.color = childColor;
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

                    // TODO Proper
                    minimapDisplay.color = Color.cyan;
                }
                else
                {
                    spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Water;
                    // TODO Proper
                    minimapDisplay.color = Color.cyan;
                }
                break;

            // A Flat Tile
            case SpaceTerrain.SpaceElevation.Flat:
                switch (terrain.baseTerrain)
                {
                    case SpaceTerrain.SpaceBaseTerrain.Desert:
                        spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Desert;
                        // TODO Proper
                        minimapDisplay.color = Color.red;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Grassland:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassForest;
                            // TODO Proper
                            minimapDisplay.color = Color.green;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassDeepForest;
                            // TODO Proper
                            minimapDisplay.color = Color.green;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Grass;
                            // TODO Proper
                            minimapDisplay.color = Color.green;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Plain:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsForest;
                            // TODO Proper
                            minimapDisplay.color = Color.yellow;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsDeepForest;
                            // TODO Proper
                            minimapDisplay.color = Color.yellow;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Plains;
                            // TODO Proper
                            minimapDisplay.color = Color.yellow;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Snow:
                        spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Snow;
                        // TODO Proper
                        minimapDisplay.color = Color.white;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Tundra:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraForest;
                            // TODO Proper
                            minimapDisplay.color = new Color(210, 105, 30);
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Tundra;
                            // TODO Proper
                            minimapDisplay.color = new Color(210, 105, 30);
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
                        // TODO Proper
                        minimapDisplay.color = Color.red;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Grassland:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHillForest;
                            // TODO Proper
                            minimapDisplay.color = Color.green;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHillDeepForest;
                            // TODO Proper
                            minimapDisplay.color = Color.green;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.GrassHill;
                            // TODO Proper
                            minimapDisplay.color = Color.green;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Plain:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHillForest;
                            // TODO Proper
                            minimapDisplay.color = Color.yellow;
                        }
                        else if (terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHillDeepForest;
                            // TODO Proper
                            minimapDisplay.color = Color.yellow;
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.PlainsHill;
                            // TODO Proper
                            minimapDisplay.color = Color.yellow;
                        }
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Snow:
                        spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.SnowHills;
                        // TODO Proper
                        minimapDisplay.color = Color.white;
                        break;

                    case SpaceTerrain.SpaceBaseTerrain.Tundra:
                        if (terrain.feature == SpaceTerrain.SpaceFeature.Forest)
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraHillForest;
                            // TODO Proper
                            minimapDisplay.color = new Color(210, 105, 30);
                        }
                        else
                        {
                            spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.TundraHill;
                            // TODO Proper
                            minimapDisplay.color = new Color(210,105,30);
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
                    // TODO Proper
                    minimapDisplay.color = Color.grey;
                }
                else
                {
                    spaceView.GetComponent<SpriteRenderer>().sprite = gameController.assets.Mountain;
                    // TODO Proper
                    minimapDisplay.color = Color.grey;
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
