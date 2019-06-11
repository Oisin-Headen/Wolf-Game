using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Model;


public class GameController : MonoBehaviour
{
    // Inserted in unity editor
    public Transform mapContainer;
    public GameObject spaceViewPrefab;
    public GameObject unitPrefab;
    public GameObject tileSelectorPrefab;
    public Assets assets;
    public Text mouseOverText;
    public CameraController cameraController;

    // TODO Won't need this eventually
    public Text currentPlayerText;

    // The game's main class
    private GameModel gameModel;



    // Start is called before the first frame update
    internal void Start()
    {
        gameModel = new GameModel(this);
    }

    internal void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            EndTurn();
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            Move();
        }
        // todo if getkeyup 'm'
        // if a moveable unit is selected, call move on it.

        // todo if getkeyup 'a'
        // if a unit with attacks is selected, call attack on it.

        // todo if getkeyup 'b'
        // if a unit that can build is selected, call build on it.

        // todo show some menus on other keys
    }

    internal void EndTurn()
    {
        gameModel.EndTurnButtonPressed();
    }

    internal void Move()
    {
        gameModel.Move();
    }


    // TODO something. Adds a SpaceModel's controller and view.
    public SpaceController AddSpace(SpaceModel spaceModel)
    {
        DoubledCoords coords = spaceModel.GetDoubledCoords();
        var x = Utilities.HEX_SIZE * Mathf.Sqrt(3) / 2 * coords.col;
        var y = Utilities.HEX_SIZE * 3 / 2f * coords.row;

        GameObject SpaceView = Instantiate(spaceViewPrefab, new Vector2(x, y),
            Quaternion.identity, mapContainer);

        SpaceView.GetComponent<SpaceController>().Setup(spaceModel, SpaceView, this);
        return SpaceView.GetComponent<SpaceController>();
    }


    // TODO Move to static method
    public UnitController AddUnit(UnitModel unitModel)
    {
        GameObject unitView = Instantiate(unitPrefab, unitModel.Space.controller.GetPosition(),
            Quaternion.identity, mapContainer);

        var unitController = unitView.GetComponent<UnitController>();
        unitController.Setup(unitModel, unitView, this);
        return unitController;
    }
}
