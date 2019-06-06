using System;using System.Collections.Generic;

public class GameModel{
    public RandomSeeds seeds;
    public MapModel map;
    private readonly Dictionary<Player, AbstractPlayer> PlayerMap;
    private Player currentPlayer;    private SpaceModel selectedSpace;    private readonly GameController gameController;    public GameModel(GameController gameController)    {        this.gameController = gameController;        seeds = new RandomSeeds();        map = new MapModel(this);        PlayerMap = new Dictionary<Player, AbstractPlayer>        {            { Player.Wolves, new WolfPlayer(this) },            { Player.Alliance, new AlliancePlayer(this) }        };        currentPlayer = Player.Wolves;        gameController.currentPlayerText.text = "Wolves";        GetCurrentPlayer().StartTurn();    }    public AbstractPlayer GetCurrentPlayer()    {        return PlayerMap[currentPlayer];    }

    public void SetSelectedSpace(SpaceModel spaceModel)    {        if (selectedSpace != null)
        {
            selectedSpace.Deselect();
        }        selectedSpace = spaceModel;    }    public void EndTurn()    {        switch (currentPlayer)        {            case Player.Alliance:                currentPlayer = Player.Wolves;                gameController.currentPlayerText.text = "Wolves";                break;            case Player.Wolves:                currentPlayer = Player.Alliance;                gameController.currentPlayerText.text = "Draconic Alliance";                break;        }    }

    public void Move()
    {
        if(selectedSpace != null)
        {
            var unit = selectedSpace.OccupingUnit;            if(unit != null && unit.GetPlayer() == currentPlayer)
            {                unit.Move();             }
        }
    }

// Methods for creating new gameobjects for unity, and linking them all together.
    // todo don't like these
    public UnitController AddUnit(UnitModel unitModel)    {        return gameController.AddUnit(unitModel);    }    public SpaceController AddSpace(SpaceModel spaceModel)    {        return gameController.AddSpace(spaceModel);    }
}