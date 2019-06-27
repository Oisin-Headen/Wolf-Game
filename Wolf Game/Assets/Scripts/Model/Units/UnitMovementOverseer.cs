using System;
using System.Collections.Generic;
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
        public bool MovingDisplayed { get; private set; }
        public bool Exploring { get; private set; }
        private List<PathfindingNode> oneTurnMovementSpaces;
        private List<SpaceModel> currentlyDispayedPath;
        private List<SpaceModel> currentTravelPath;

        //private List<SpaceModel> exploredTravelPath;
        //private int spaceAlongExploredPath;

        private SpaceModel destination;

        public UnitMovementOverseer(UnitModel unit, GameModel gameModel)
        {
            this.unit = unit;
            this.gameModel = gameModel;
        }

        internal void ShowMove()
        {
            if (MovingDisplayed)
            {
                HideMove();
                return;
            }
            MovingDisplayed = true;
            oneTurnMovementSpaces = PathfindingDijkstras.GetSpacesForMovementDijkstras(
                unit.Space, unit.CurrentMovement, unit.UnitType.MovementCostDeterminer);
            foreach (var node in oneTurnMovementSpaces)
            {
                node.Space.controller.SetMoveable();
            }
        }

        // Does the player need to give this unit orders?
        internal bool HasQueuedActions()
        {
            return Fortified || Exploring || destination != null;
        }

        internal void HideMove()
        {
            MovingDisplayed = false;
            if (oneTurnMovementSpaces != null)
            {
                foreach (var node in oneTurnMovementSpaces)
                {
                    node.Space.controller.Deselect();
                    unit.Space.controller.SetSelected();
                }
            }
            if (currentlyDispayedPath != null)
            {
                foreach (var pathSpace in currentlyDispayedPath)
                {
                    pathSpace.controller.Deselect();
                }
            }
        }

        internal void HoverSpace(SpaceModel space)
        {
            if (MovingDisplayed)
            {
                var newPath = AStarPathfinding.GetPathToDestination(unit.Space, space,
                    unit.UnitType.MovementCostDeterminer);

                // The Mouse has moved somewhere else
                if (space != gameModel.CurrentMousePosition || !MovingDisplayed)
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
        }

        internal void ClickSpace(SpaceModel space)
        {
            if (MovingDisplayed)
            {
                destination = space;
                Fortified = false;
                Exploring = false;
                unit.controller.RevertBackground();
                HideMove();
                gameModel.SelectedUnit = null;
                unit.Space.controller.Deselect();
                if (currentlyDispayedPath != null)
                {
                    foreach (var pathSpace in currentlyDispayedPath)
                    {
                        pathSpace.controller.Deselect();
                    }
                }
                currentTravelPath = currentlyDispayedPath;
                TravelAlongPath();
            }
        }

        internal void Fortify()
        {
            unit.Space.Deselect();
            HideMove();
            Exploring = false;
            Fortified = true;
            destination = null;
            unit.controller.SetBackGroundShape(Assets.UnitBackgrounds.Shield);
        }

        internal void Explore()
        {
            unit.Space.Deselect();
            HideMove();
            Fortified = false;
            Exploring = true;
            unit.controller.RevertBackground();
            //destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
            //TravelAlongPath();
        }


        // Returns true if done, false if threading.
        internal bool TryEndTurn()
        {
            if (Fortified)
            {
                MovementTask.MarkComplete();
                return true;
            }
            if (unit.CurrentMovement == 0)
            {
                MovementTask.MarkComplete();
                return true;
            }
            if (unit.Space == destination)
            {
                destination = null;
                
            }

            if (Exploring)
            {

                gameModel.EndTurnUnitMoving();
                Thread thread = new Thread(() =>
                {
                    var closestUnexplored = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);


                    bool continuing = true;
                    if (closestUnexplored == null)
                    {
                        Exploring = false;
                        continuing = false;
                    }

                    while (!MovementTask.Complete() && continuing)
                    {
                        List<SpaceModel> newPath = AStarPathfinding.GetPathToDestination(unit.Space, closestUnexplored,
                               unit.UnitType.MovementCostDeterminer);

                        currentTravelPath = newPath;

                        UnityToolbag.Dispatcher.Invoke(TravelAlongPath);

                        if (!MovementTask.Complete())
                        {
                            closestUnexplored = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
                            if (closestUnexplored == null)
                            {
                                Exploring = false;
                                continuing = false;
                            }
                        }
                    }
                    UnityToolbag.Dispatcher.InvokeAsync(() =>
                    {
                        gameModel.EndTurnUnitMoved();
                    });
                });
                thread.Start();
                return false;
            }
            else
            {
                if (destination == null)
                {
                    return true;
                }

                gameModel.EndTurnUnitMoving();
                Thread thread = new Thread(() =>
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
                            gameModel.EndTurnUnitMoved();
                        }
                    });
                });
                thread.Start();
                return false;
            }
        }

        public void TravelAlongPath()
        {
            int spaceAlongPath = 0;
            bool continuing = true;
            if (currentTravelPath == null)
            {
                return;
            }
            while (unit.CurrentMovement > 0 && continuing)
            {
                try
                {
                    var nextSpace = currentTravelPath[spaceAlongPath];
                    if (!nextSpace.Occupied() &&
                        unit.UnitType.MovementCostDeterminer.GetMovementCost(nextSpace) != -1)
                    {
                        unit.Enter(nextSpace);
                        spaceAlongPath++;
                    }
                    else
                    {
                        continuing = false;
                    }
                    continuing &= unit.Space != destination;
                    if (unit.Space == destination)
                    {
                        destination = null;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    continuing = false;
                    MovementTask.MarkComplete();
                }
            }
            if (unit.CurrentMovement == 0)
            {
                MovementTask.MarkComplete();
            }
        }
    }
}
