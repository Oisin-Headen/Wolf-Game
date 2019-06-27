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

        private List<SpaceModel> exploredTravelPath;
        private int spaceAlongExploredPath;

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
            HideMove();
            Exploring = false;
            Fortified = true;
            destination = null;
            unit.controller.SetBackGroundShape(Assets.UnitBackgrounds.Shield);
        }

        internal void Explore()
        {
            HideMove();
            Fortified = false;
            unit.controller.RevertBackground();
            destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
            TravelAlongPath();
        }


        // Returns true if done, false if threading.
        internal bool TryEndTurn()
        {
            //while (unit.CurrentMovement > 0)
            //{
            //    if (unit.Space == destination)
            //    {
            //        if (Exploring)
            //        {
            //            destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
            //            CalculatePath();
            //        }
            //        else
            //        {
            //            destination = null;
            //            break;
            //        }
            //    }

            //    if (exploredTravelPath != null)
            //    {
            //        TravelAlongPath();
            //    }
            //    else
            //    {
            //        CalculatePath();
            //    }
            //}

            //if(unit.CurrentMovement == 0)
            //{
            //    MovementTask.MarkComplete();
            //}
            //return true;

            //gameModel.EndTurnUnitMoving();

            if (Fortified)
            {
                MovementTask.MarkComplete(); return true;
            }
            if (unit.CurrentMovement == 0)
            {
                MovementTask.MarkComplete(); return true;
            }
            if (unit.Space == destination)
            {
                destination = null;
                if (Exploring)
                {
                    destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
                }
            }
            if (destination == null) { return true; }
            gameModel.EndTurnUnitMoving(); Thread thread = new Thread(() =>
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
                        if (unit.Space == destination)
                        {
                            destination = PathfindingDijkstras.GetClosestUnexplored(unit.Space, unit.UnitType.MovementCostDeterminer);
                            currentTravelPath = AStarPathfinding.GetPathToDestination(unit.Space, destination,
                                unit.UnitType.MovementCostDeterminer);
                            TravelAlongPath();
                        }
                        gameModel.EndTurnUnitMoved();
                    }
                });
            });
            thread.Start();
            return false;
        }

        private void CalculatePath()
        {
            List<SpaceModel> newPath = AStarPathfinding.GetPathToDestination(unit.Space, destination,
                       unit.UnitType.MovementCostDeterminer);

            if(newPath != null)
            {
                List<SpaceModel> exploredPath = new List<SpaceModel>();

                int alongPath = 0;
                while(!newPath[alongPath].Explored)
                {
                    exploredPath.Add(newPath[alongPath]);
                    alongPath++;
                }
                spaceAlongExploredPath = 0;
                exploredTravelPath = exploredPath;
            }

            exploredTravelPath = null;
        }

        public void TravelAlongPath()
        {
            bool continuing = true;
            if (exploredTravelPath == null)
            {
                return;
            }
            while (unit.CurrentMovement > 0 && continuing)
            {
                var nextSpace = exploredTravelPath[spaceAlongExploredPath];
                if (!nextSpace.Occupied() &&
                    unit.UnitType.MovementCostDeterminer.GetMovementCost(nextSpace) != -1)
                {
                    unit.Enter(nextSpace);
                    spaceAlongExploredPath++;
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
            if (unit.CurrentMovement == 0)
            {
                MovementTask.MarkComplete();
            }
        }
    }
}
