using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using UnityEngine;

namespace Pathfinding
{
    public static class AStarPathfinding
    {
        public static List<SpaceModel> GetPathToDestination(SpaceModel startSpace, SpaceModel destSpace, IMovementCost costDeterminer)
        {
            if (costDeterminer.GetMovementCost(destSpace) == -1)
            {
                return null;
            }

            Dictionary<SpaceModel, PathfindingNode> allNodes = new Dictionary<SpaceModel, PathfindingNode>();

            PathfindingNode currentnode = new PathfindingNode(startSpace, null, 0, true, destSpace);
            allNodes[startSpace] = currentnode;
            bool done = false;
            while (!done)
            {
                foreach (SpaceModel adjacentSpace in currentnode.Space.GetAdjacentSpaces())
                {
                    if (adjacentSpace != null)
                    {
                        try
                        {
                            PathfindingNode nextNode = allNodes[adjacentSpace];
                            int newNodeCost = currentnode.Cost + costDeterminer.GetMovementCost(adjacentSpace);
                            double newPathfindingCost = newNodeCost;
                            if (!nextNode.Seen)
                            {
                                // Next node hasn't been visited yet
                                //nextNode.Update(newNodeCost, newPathfindingCost, currentnode);
                                nextNode.Update(newNodeCost, currentnode);
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            // Is null, need new node
                            if (costDeterminer.GetMovementCost(adjacentSpace) != -1)
                            {
                                int newNodeCost = currentnode.Cost + costDeterminer.GetMovementCost(adjacentSpace);
                                PathfindingNode newNode = new PathfindingNode(adjacentSpace, currentnode, newNodeCost, false, destSpace);
                                allNodes[adjacentSpace] = newNode;
                            }
                        }
                    }
                }
                currentnode.SetSeen();
                if (!currentnode.Space.Equals(destSpace))
                {
                    PathfindingNode lowestNode = null;
                    foreach (PathfindingNode node in allNodes.Values)
                    {
                        if (!node.Seen)
                        {
                            if (lowestNode == null)
                            {
                                lowestNode = node;
                            }
                            else
                            {
                                if (node.ASTotal <= lowestNode.ASTotal)
                                {
                                    if (node.ASRemaining <= lowestNode.ASRemaining)
                                    {
                                        lowestNode = node;
                                    }
                                }
                            }
                        }
                    }
                    if (lowestNode == null)
                    {
                        done = true;
                    }
                    else
                    {
                        currentnode = lowestNode;
                    }
                }
                else
                {
                    done = true;
                }
            }

            if (currentnode.Space != destSpace)
            {
                return null;
            }

            List<SpaceModel> path = new List<SpaceModel>();
            PathfindingNode backtrackNode = currentnode;
            while (backtrackNode != null)
            {
                path.Add(backtrackNode.Space);
                backtrackNode = backtrackNode.GetParent();
            }
            path.Reverse();

            path.Remove(startSpace);
            return path;
        }
    }
}
