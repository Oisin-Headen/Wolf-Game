﻿using System;

public class GameModel
    public RandomSeeds seeds;
    public MapModel map;
    private readonly Dictionary<Player, AbstractPlayer> PlayerMap;
    private Player currentPlayer;

    public UnitModel SelectedUnit { get; private set; }

    public GameModel(GameController gameController)

    public void SetSelectedUnit(UnitModel unitModel)
        {
            SelectedUnit.Space.Deselect();
        }
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
        }
            GetCurrentPlayer().PreEndTurnTask();

    public void Move()
    {
        if(SelectedUnit != null)
        {
            if(SelectedUnit.GetPlayer() == currentPlayer)
            {
                // Todo for figuring out pathfinding, leave commented for now.
                //var thread = new System.Threading.Thread(() => SelectedUnit.StartMove());
                //thread.Start();
                SelectedUnit.StartMove(); 
        }
    }


    // todo don't like these
    public UnitController AddUnit(UnitModel unitModel)
}