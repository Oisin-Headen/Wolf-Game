using System;using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace Model
{
    public class UnitMovementOverseer
    {
        internal readonly UnitModel unit;
        private readonly GameModel gameModel;

        public PreEndTurnMovementTask MovementTask { get; internal set; }
        public bool Fortified { get; private set; }
        public bool Moving { get; private set; }        public bool Exploring { get; private set; }        private List<PathfindingNode> oneTurnMovementSpaces;
        private List<SpaceModel> currentlyDispayedPath;        private List<SpaceModel> currentTravelPath;        private SpaceModel destination;
        public UnitMovementOverseer(UnitModel unit, GameModel gameModel)
        {            this.unit = unit;            this.gameModel = gameModel;
        }        internal void ShowMove()
        {
            if (Moving)
            {
                HideMove();                return;
            }            Moving = true;            oneTurnMovementSpaces = PathfindingDijkstras.GetSpacesForMovementDijkstras(                unit.Space, unit.CurrentMovement, unit.UnitType.MovementCostDeterminer);            foreach (var node in oneTurnMovementSpaces)
            {                node.Space.controller.SetMoveable();
            }
        }

        // Does the player need to give this unit orders?
        internal bool HasQueuedActions()
        {
            return Fortified || destination != null || Exploring;
        }

        internal void HideMove()        {            Moving = false;            if (oneTurnMovementSpaces != null)
            {
                foreach (var node in oneTurnMovementSpaces)
                {
                    node.Space.controller.Deselect();
                    unit.Space.controller.SetSelected();
                }            }        }        internal void HoverSpace(SpaceModel space)        {            if (Moving)
            {
                var newPath = AStarPathfinding.GetPathToDestination(unit.Space, space,
                    unit.UnitType.MovementCostDeterminer);

                // The Mouse has moved somewhere else
                if (space != gameModel.CurrentMousePosition || !Moving)
                {
                    return;
                }

                if (currentlyDispayedPath != null)
                {
                    foreach (var pathSpace in currentlyDispayedPath)
                    {
                        pathSpace.controller.SetPath(false);
                    }
                }

                currentlyDispayedPath = newPath;

                if (currentlyDispayedPath != null)
                {
                    foreach (var pathSpace in currentlyDispayedPath)
                    {
                        pathSpace.controller.SetPath(true);
                    }
                }
            }        }        internal void ClickSpace(SpaceModel space)        {            if (Moving)            {                destination = space;                Fortified = false;
                Exploring = false;                unit.controller.RevertBackground();                HideMove();                Moving = false;                gameModel.SelectedUnit = null;                unit.Space.controller.Deselect();                if (currentlyDispayedPath != null)                {                    foreach (var pathSpace in currentlyDispayedPath)                    {                        pathSpace.controller.Deselect();                    }                }                currentTravelPath = currentlyDispayedPath;                TravelAlongPath();            }        }        internal void Fortify()        {            HideMove();            Exploring = false;            Fortified = true;            destination = null;            unit.controller.SetBackGroundShape(Assets.UnitBackgrounds.Shield);        }        internal void Explore()
        {
            HideMove();
            Fortified = false;
            unit.controller.RevertBackground();
            destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
            TravelAlongPath();
        }        // Returns true if done, false if threading.        internal bool TryEndTurn()
        {            if (Fortified)
            {
                MovementTask.MarkComplete();                return true;
            }            if (unit.CurrentMovement == 0)
            {                MovementTask.MarkComplete();                return true;
            }            if (unit.Space == destination)
            {
                destination = null;
                if(Exploring)
                {
                    destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
                }
            }            if (destination == null)            {                return true;            }            gameModel.EndTurnUnitMoving();            Thread thread = new Thread(() =>
            {
                List<SpaceModel> newPath = AStarPathfinding.GetPathToDestination(unit.Space, destination,
                       unit.UnitType.MovementCostDeterminer);

                UnityToolbag.Dispatcher.InvokeAsync(() =>
                {
                    if (newPath == null)
                    {
                        destination = null;
                        gameModel.EndTurnUnitMoved();
                    }
                    else
                    {
                        currentTravelPath = newPath;

                        TravelAlongPath();
                        if(unit.Space == destination)
                        {
                            destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
                            currentTravelPath = AStarPathfinding.GetPathToDestination(unit.Space, destination,
                                unit.UnitType.MovementCostDeterminer);
                            TravelAlongPath();
                        }
                        gameModel.EndTurnUnitMoved();
                    }
                });
            });            thread.Start();            return false;        }        public void TravelAlongPath()
        {
            int pathSpaceNum = 0;            bool continuing = true;            if(currentTravelPath == null)
            {
                return;
            }            while (unit.CurrentMovement > 0 && continuing)            {                var nextSpace = currentTravelPath[pathSpaceNum];                if (!nextSpace.Occupied() &&                    unit.UnitType.MovementCostDeterminer.GetMovementCost(nextSpace) != -1)                {                    unit.Enter(nextSpace);                    pathSpaceNum++;                }                else                {                    continuing = false;                }                continuing &= unit.Space != destination;                if (unit.Space == destination)
                {
                    destination = null;
                }            }            if (unit.CurrentMovement == 0)
            {
                MovementTask.MarkComplete();
            }
        }
    }
}


//public void StartMove()
//{
//    //moving = true;
//    movementSpaces = PathfindingDijkstras.GetSpacesForMovementDijkstras(
//        Space, CurrentMovement, UnitType.MovementCostDeterminer);


//    foreach (var node in movementSpaces)
//    {
//        node.Space.Moveable();
//        //node.GetSpace().controller.SetText(node.GetCost());
//    }
//    gameModel.Moving = true;
//}

//public void FinishMove(SpaceModel spaceModel)
//{
//    foreach (var node in movementSpaces)
//    {
//        if (node.Space == spaceModel)
//        {
//            CurrentMovement = Math.Max(0, CurrentMovement - node.Cost);
//            controller.MovePosition(node.Space);
//            Space.OccupingUnit = null;
//            Space = node.Space;
//            Space.OccupingUnit = this;
//        }
//        node.Space.Deselect();
//    }
//    movementSpaces = null;
//    gameModel.SetSelectedUnit(null);
//    Explore();

//    if (CurrentMovement <= 0)
//    {
//        MovementTask.MarkComplete();
//    }

//    gameModel.Moving = false;
//    gameModel.FinishedMovement();
//}
