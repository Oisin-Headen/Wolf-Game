using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour
{
    // Inserted in unity editor
    public Transform mapContainer;
    public GameObject spaceViewPrefab;
    public GameObject unitPrefab;
    public Assets assets;
    public Text mouseOverText;

    // TODO Won't need this eventually
    public Text currentPlayerText;

    public RandomSeeds seeds;

    // Our Reference to the Map
    public MapModel map;
    private Player currentPlayer;
    private Dictionary<Player, AbstractPlayer> PlayerMap;

    // Start is called before the first frame update
    void Start()
    {
        seeds = new RandomSeeds();
        map = new MapModel(this);

        PlayerMap = new Dictionary<Player, AbstractPlayer>
        {
            { Player.Wolves, new WolfPlayer(this) },
            { Player.Alliance, new AlliancePlayer(this) }
        };

        currentPlayer = Player.Wolves;
        currentPlayerText.text = "Wolves";
        GetCurrentPlayer().StartTurn();
    }

    // Adds a SpaceModel's controller and view.
    public SpaceController AddSpace(SpaceModel spaceModel)
    {
        DoubledCoords coords = spaceModel.GetDoubledCoords();
        var x = Utilities.HEX_SIZE * Mathf.Sqrt(3)/2 * coords.col;
        var y = Utilities.HEX_SIZE * 3/2f * coords.row;

        GameObject SpaceView = Instantiate(spaceViewPrefab, new Vector2(x, y), Quaternion.identity, mapContainer);
        SpaceView.GetComponent<SpaceController>().SetSpaceView(spaceModel, SpaceView, this);
        return SpaceView.GetComponent<SpaceController>();
    }

    public UnitController AddUnit(UnitModel unitModel)
    {
        GameObject unitView = Instantiate(unitPrefab, unitModel.Space.controller.GetPosition(), Quaternion.identity, mapContainer);
        var unitController = unitView.GetComponent<UnitController>();
        unitController.Setup(unitModel, unitView, this);
        return unitController;
    }

    public AbstractPlayer GetCurrentPlayer()
    {
        return PlayerMap[currentPlayer];
    }

    // Method Called when the turn is ended.
    public void EndTurn()
    {
        switch(currentPlayer)
        {
            case Player.Alliance:
                currentPlayer = Player.Wolves;
                currentPlayerText.text = "Wolves";
                break;
            case Player.Wolves:
                currentPlayer = Player.Alliance;
                currentPlayerText.text = "Draconic Alliance";
                break;
        }
    }
}