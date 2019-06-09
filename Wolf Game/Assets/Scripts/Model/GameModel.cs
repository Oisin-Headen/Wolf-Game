using System;using System.Collections.Generic;

public class GameModel{
    public RandomSeeds seeds;
    public MapModel map;
    private readonly Dictionary<Player, AbstractPlayer> PlayerMap;
    private Player currentPlayer;        private readonly GameController gameController;

    public UnitModel SelectedUnit { get; private set; }

    public GameModel(GameController gameController)    {        this.gameController = gameController;        seeds = new RandomSeeds();        map = new MapModel(this);        List<SpaceModel> deepforests = map.GetDeepForests();        PlayerMap = new Dictionary<Player, AbstractPlayer>        {            { Player.Wolves, new WolfPlayer(this, deepforests) },            { Player.Alliance, new AlliancePlayer(this) }        };        currentPlayer = Player.Wolves;        gameController.currentPlayerText.text = "Wolves";        GetCurrentPlayer().StartTurn();    }    public AbstractPlayer GetCurrentPlayer()    {        return PlayerMap[currentPlayer];    }

    public void SetSelectedUnit(UnitModel unitModel)    {        if (SelectedUnit != null)
        {
            SelectedUnit.Space.Deselect();
        }        SelectedUnit = unitModel;    }    public void EndTurnButtonPressed()    {        if (GetCurrentPlayer().CanEndTurn())
        {
            //switch (currentPlayer)
            //{
            //    case Player.Alliance:
            //        currentPlayer = Player.Wolves;
            //        gameController.currentPlayerText.text = "Wolves";
            //        break;
            //    case Player.Wolves:
            //        currentPlayer = Player.Alliance;
            //        gameController.currentPlayerText.text = "Draconic Alliance";
            //        break;
            //}

            GetCurrentPlayer().StartTurn();
        }        else         {
            GetCurrentPlayer().PreEndTurnTask();        }    }

    public void Move()
    {
        if(SelectedUnit != null)
        {
            if(SelectedUnit.GetPlayer() == currentPlayer)
            {
                // Todo for figuring out pathfinding, leave commented for now.
                //var thread = new System.Threading.Thread(() => SelectedUnit.StartMove());
                //thread.Start();
                SelectedUnit.StartMove();             }
        }
    }

// Methods for creating new gameobjects for unity, and linking them all together.
    // todo don't like these
    public UnitController AddUnit(UnitModel unitModel)    {        return gameController.AddUnit(unitModel);    }    public SpaceController AddSpace(SpaceModel spaceModel)    {        return gameController.AddSpace(spaceModel);    }
}