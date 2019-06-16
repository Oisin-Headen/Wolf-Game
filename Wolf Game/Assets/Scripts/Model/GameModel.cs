﻿using System;using System.Collections.Generic;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace Model{
    public class GameModel
    {
        public RandomSeeds seeds;
        public MapModel map;
        private readonly Dictionary<PlayerType, APlayer> PlayerMap;
        private PlayerType currentPlayer;
        private readonly GameController gameController;

        public UnitModel SelectedUnit { get; internal set; }
        public bool Moving { get; internal set; }
        public SpaceModel CurrentMousePosition { get; private set; }        private int movingUnits;

        //private List<SpaceModel> currentlyDispayedPath;
        public GameModel(GameController gameController)
        {
            this.gameController = gameController;

            seeds = new RandomSeeds();
            map = new MapModel(this);

            List<SpaceModel> deepforests = map.GetDeepForests();

            PlayerMap = new Dictionary<PlayerType, APlayer>            {                { PlayerType.Wolves, new WolfPlayer(this, deepforests) },                { PlayerType.Alliance, new AlliancePlayer(this) }            };

            currentPlayer = PlayerType.Wolves;
            gameController.currentPlayerText.text = "Wolves";
            GetCurrentPlayer().StartTurn();
        }

        public APlayer GetCurrentPlayer()
        {
            return PlayerMap[currentPlayer];
        }

        public void EndTurnButtonPressed()
        {            Debug.Log(movingUnits);            movingUnits = 0;            //gameController.SetMainButtonText("Please Wait");

            Debug.Log("End Turn Button");

            if (GetCurrentPlayer().CanEndTurn())
            {
                EndTurn();
            }
            else
            {
                switch(GetCurrentPlayer().TryEndTurn())
                {
                    case EndTurnButtonResult.Normal:
                        EndTurn();
                        break;
                    case EndTurnButtonResult.Show_Task:
                        GetCurrentPlayer().PreEndTurnTask();
                        break;
                    case EndTurnButtonResult.Threading:
                        gameController.SetMainButtonText("Please Wait, Threading");
                        // Do Nothing
                        break;
                }
            }
        }        public void EndTurn()
        {
            // TODO end turn cycle here
            gameController.SetMainButtonText("End Turn");
            GetCurrentPlayer().StartTurn();
            Debug.Log("End Turn Real");
        }        internal void EndTurnUnitMoved()        {            Debug.Log("End Turn Moved");            --movingUnits;            if(movingUnits == 0)
            {                if (GetCurrentPlayer().CanEndTurn())
                {
                    gameController.SetMainButtonText("End Turn");                    EndTurn();
                }                else                {                    GetCurrentPlayer().PreEndTurnTask();                }            }        }        internal void EndTurnUnitMoving()        {            Debug.Log("End Turn Button");            ++movingUnits;        }

        internal void Clicked(SpaceModel spaceModel)
        {
            if(SelectedUnit == null)
            {
                SelectedUnit = spaceModel.OccupingUnit;                if (SelectedUnit != null)
                {
                    SelectedUnit.Space.controller.SetSelected();
                }
            }            else if(SelectedUnit.MovementOverseer.Moving)
            {                SelectedUnit.MovementOverseer.ClickSpace(spaceModel);             }            else
            {                SelectedUnit.Space.controller.Deselect();                SelectedUnit = null;                if (spaceModel.Occupied())
                {
                    SelectedUnit = spaceModel.OccupingUnit;
                    spaceModel.controller.SetSelected();
                }            }        }        internal void HoverOverSpace(SpaceModel spaceModel)        {            CurrentMousePosition = spaceModel;            if (SelectedUnit != null)            {                SelectedUnit.MovementOverseer.HoverSpace(spaceModel);            }        }

        public void Move()
        {
            if (SelectedUnit != null)
            {
                if (SelectedUnit.GetPlayer() == currentPlayer)
                {
                    SelectedUnit.MovementOverseer.ShowMove();
                }
            }
        }


        // Methods for creating new gameobjects for unity, and linking them all together.
        // todo don't like these
        public UnitController AddUnit(UnitModel unitModel)
        {
            return gameController.AddUnit(unitModel);
        }

        public SpaceController AddSpace(SpaceModel spaceModel)
        {
            return gameController.AddSpace(spaceModel);
        }
    }
}