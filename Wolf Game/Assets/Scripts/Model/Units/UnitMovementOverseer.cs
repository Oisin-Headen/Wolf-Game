﻿using System;
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
        public bool Moving { get; internal set; }
        private List<SpaceModel> currentlyDispayedPath;
        public UnitMovementOverseer(UnitModel unit, GameModel gameModel)
        {
        }
        {
            if (Moving)
            {
                HideMove();
            }
            {
            }
        }
            {
                foreach (var node in oneTurnMovementSpaces)
                {
                    node.Space.controller.Deselect();
                    unit.Space.controller.SetSelected();
                }
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
            }
        {
            {
                MovementTask.MarkComplete();
            }
            {
            }
            {
                destination = null;
            }
            {
                List<SpaceModel> newPath = AStarPathfinding.GetPathToDestination(unit.Space, destination,
                       unit.UnitType.MovementCostDeterminer);

                if (newPath == null)
                {
                    destination = null;
                    gameModel.EndTurnUnitMoved();
                    return;
                }
                UnityToolbag.Dispatcher.InvokeAsync(() =>
                {
                    currentTravelPath = newPath;

                    TravelAlongPath();
                });
            });
        {
            int pathSpaceNum = 0;
            {
                return;
            }
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