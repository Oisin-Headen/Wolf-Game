using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace Model
{
    public class GameModel
    {
        public RandomSeeds seeds;
        public MapModel map;
        private readonly Dictionary<PlayerType, APlayer> PlayerMap;
        private PlayerType currentPlayer;
        private readonly GameController gameController;

        public UnitModel SelectedUnit { get; internal set; }
        public bool Moving { get; internal set; }
        public SpaceModel CurrentMousePosition { get; private set; }
        private int movingUnits;


        public int TurnNumber = 0;

        //private List<SpaceModel> currentlyDispayedPath;
        public GameModel(GameController gameController)
        {
            this.gameController = gameController;

            seeds = new RandomSeeds();
            map = new MapModel(this);

            List<SpaceModel> deepforests = map.GetDeepForests();

            PlayerMap = new Dictionary<PlayerType, APlayer> {
                { PlayerType.Wolves, new WolfPlayer(this, deepforests) },
                { PlayerType.Alliance, new AlliancePlayer(this) }
            };

            currentPlayer = PlayerType.Wolves;
            gameController.currentPlayerText.text = "Wolves : 1";
            EndTurn();
        }

        public APlayer GetCurrentPlayer()
        {
            return PlayerMap[currentPlayer];
        }

        public void MainButtonPressed()
        {
            if (GetCurrentPlayer().NeedsOrders())
            {
                GetCurrentPlayer().ShowTaskNeedingOrders();
                gameController.SetMainButton("A Unit Needs Orders", true);
            }
            else
            {
                movingUnits = 0;
                switch (GetCurrentPlayer().ExecuteRemainingTasks())
                {
                    case EndTurnButtonResult.Normal:
                        EndTurn();
                        break;
                    case EndTurnButtonResult.Show_Task:
                        gameController.SetMainButton("A Unit Needs Orders", true);
                        GetCurrentPlayer().PreEndTurnTask();
                        break;
                    case EndTurnButtonResult.Threading:
                        gameController.SetMainButton("Please Wait...", false);
                        // Do Nothing
                        break;
                }
            }
        }
        public void EndTurn()
        {
            gameController.SetMainButton("Please Wait...", false);
            // TODO end turn cycle here
            TurnNumber++;

            gameController.currentPlayerText.text = "Wolves : " + TurnNumber;

            GetCurrentPlayer().StartTurn();

            if (!GetCurrentPlayer().NeedsOrders())
            {
                gameController.SetMainButton("End Turn", true);
            }
            else
            {
                gameController.SetMainButton("A Unit Needs Orders", true);
            }
        }

        internal void EndTurnUnitMoved()
        {
            --movingUnits; if (movingUnits == 0)
            {
                if (GetCurrentPlayer().CanEndTurn())
                {
                    EndTurn();
                }
                else
                {
                    gameController.SetMainButton("A Unit Needs Orders", true); GetCurrentPlayer().PreEndTurnTask();
                }
            }
        }
        internal void EndTurnUnitMoving() { ++movingUnits; }

        internal void Clicked(SpaceModel spaceModel)
        {
            if (SelectedUnit == null)
            {
                SelectedUnit = spaceModel.OccupingUnit; if (SelectedUnit != null)
                {
                    SelectedUnit.Space.controller.SetSelected();
                }
            }
            else if (SelectedUnit.MovementOverseer.MovingDisplayed)
            { SelectedUnit.MovementOverseer.ClickSpace(spaceModel); }
            else
            {
                SelectedUnit.Space.controller.Deselect(); SelectedUnit = null; if (spaceModel.Occupied())
                {
                    SelectedUnit = spaceModel.OccupingUnit;
                    spaceModel.controller.SetSelected();
                }
            }
        }
        internal void HoverOverSpace(SpaceModel spaceModel) { CurrentMousePosition = spaceModel; if (SelectedUnit != null) { SelectedUnit.MovementOverseer.HoverSpace(spaceModel); } }

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

        public void Fortify()
        {
            if (SelectedUnit != null)
            {
                if (SelectedUnit.GetPlayer() == currentPlayer)
                {
                    SelectedUnit.MovementOverseer.Fortify();
                }
            }
        }

        public void Explore()
        {
            if (SelectedUnit != null)
            {
                if (SelectedUnit.GetPlayer() == currentPlayer)
                {
                    SelectedUnit.MovementOverseer.Explore();
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

        //todo text method
        public void ExploreAll()
        {
            map.ExploreAll();
        }
    }
}
