using System;using System.Collections.Generic;
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
        public bool Moving { get; internal set; }        private List<PathfindingNode> oneTurnMovementSpaces;
        private List<SpaceModel> currentlyDispayedPath;        private SpaceModel destination;
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
        }        internal void HideMove()        {            Moving = false;            foreach (var node in oneTurnMovementSpaces)
            {                node.Space.controller.Deselect();                unit.Space.controller.SetSelected();
            }        }        internal void HoverSpace(SpaceModel space)        {            if (Moving)
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
            }        }        internal void ClickSpace(SpaceModel space)        {            if (Moving)            {                destination = space;                Fortified = false;                unit.controller.RevertBackground();                HideMove();                Moving = false;                gameModel.SelectedUnit = null;                unit.Space.controller.Deselect();                TryEndTurn();            }        }        internal void Fortify()        {            HideMove();            Fortified = true;            destination = null;            unit.controller.SetBackGroundShape(Assets.UnitBackgrounds.Shield);        }        internal void TryEndTurn()
        {            if (Fortified)
            {
                MovementTask.MarkComplete();                return;
            }            if (unit.CurrentMovement == 0)
            {                MovementTask.MarkComplete();                return;
            }            if(unit.Space == destination)
            {
                destination = null;
            }            if (destination == null)            {                return;            }
            List<SpaceModel> newPath = AStarPathfinding.GetPathToDestination(unit.Space, destination,
                   unit.UnitType.MovementCostDeterminer);            if (newPath == null)
            {                destination = null;                MovementTask.MarkComplete();
                return;
            }            int pathSpaceNum = 0;            bool continuing = true;            while (unit.CurrentMovement > 0 && continuing)
            {
                if (!newPath[pathSpaceNum].Occupied() &&
                    unit.UnitType.MovementCostDeterminer.GetMovementCost(newPath[pathSpaceNum]) != -1)
                {
                    unit.Enter(newPath[pathSpaceNum]);
                    pathSpaceNum++;
                }
                else
                {
                    continuing = false;
                }
                continuing &= unit.Space != destination;
            }            MovementTask.MarkComplete();        }
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