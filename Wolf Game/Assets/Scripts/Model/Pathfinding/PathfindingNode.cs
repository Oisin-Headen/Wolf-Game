using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;

namespace Pathfinding
{
    public class PathfindingNode
    {
        // A* only variable. Guess of remaining distance
        private readonly int aSRemaining;
        internal int ASRemaining => aSRemaining;
        internal int ASTotal => aSRemaining + Cost;

        private PathfindingNode parent;

        internal int Cost { get; private set; }
        internal bool Seen { get; private set; }

        public SpaceModel Space { get; }


        // Create a node on a space. Special constructor for A* nodes, includes guess of remaining distance
        public PathfindingNode(SpaceModel space, PathfindingNode parent, int cost, bool seen, SpaceModel destination)
        {
            Space = space;
            Cost = cost;
            this.parent = parent;
            Seen = seen;

            if (destination != null)
            {
                // Spaces Up/Down (UD) +
                // Spaces Side - (UD) / 2 (Min 0)
                // This section gets the minimum distance between two points on our hex map.
                var coords = space.GetDoubledCoords();
                int vertical = Math.Abs(coords.row - coords.row);
                int furtherHorizontal = Math.Max(Math.Abs(coords.col - coords.col) - vertical, 0) / 2;
                // Multiply remaining Spaces by space cost constant
                aSRemaining = (vertical + furtherHorizontal) * PathfindingDijkstras.ONE_SPACE;
            }
        }

        internal PathfindingNode GetParent()
        {
            return parent;
        }

        // This node has been visited
        internal void SetSeen()
        {
            Seen = true;
        }

        // recursive function
        public List<PathfindingNode> GetPath(bool endPoint)
        {
            if (parent == null)
            {
                return new List<PathfindingNode>();
            }
            else if (endPoint)
            {
                List<PathfindingNode> path = parent.GetPath(false);
                path.Add(parent);
                path.Add(this);
                return path;
            }
            else
            {
                List<PathfindingNode> path = parent.GetPath(false);
                path.Add(parent);
                return path;
            }
        }

        // Is this new path better than the current one? If yes, replace the current one.
        internal void Update(int newCost, PathfindingNode newParent)
        {
            if (newCost < Cost)
            {
                Cost = newCost;
                //pathfindingCost = newPathfindingCost;
                parent = newParent;
            }
        }
    }
}