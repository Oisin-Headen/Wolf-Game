using System;using System.Collections.Generic;
using System.Threading.Tasks;
using Pathfinding;
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
        public SpaceModel CurrentMousePosition { get; private set; }

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
        {
            if (GetCurrentPlayer().TryEndTurn())
            {                // todo put turn cycle in here.
                GetCurrentPlayer().StartTurn();
            }
            else
            {
                GetCurrentPlayer().PreEndTurnTask();
            }
        }

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
                }            }
        }

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

        internal void HoverOverSpace(SpaceModel spaceModel)
        {            CurrentMousePosition = spaceModel;            if (SelectedUnit != null)
            {
                SelectedUnit.MovementOverseer.HoverSpace(spaceModel);
            }
        }        //internal void FinishedMovement()
        //{
        //    if (currentlyDispayedPath != null)        //    {        //        foreach (var space in currentlyDispayedPath)        //        {        //            space.controller.Deselect();        //        }        //    }        //}
    }
}