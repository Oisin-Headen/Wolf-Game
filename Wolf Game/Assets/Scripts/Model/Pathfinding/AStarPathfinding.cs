using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public static class AStarPathfinding
    {
        public static List<SpaceModel> GetPathToDestination(SpaceModel startSpace, SpaceModel destSpace, IMovementCost costDeterminer)
        {
            List<PathfindingNode> allNodes = new List<PathfindingNode>();

            PathfindingNode currentnode = new PathfindingNode(startSpace, null, 0, true, destSpace);
            allNodes.Add(currentnode);
            bool done = false;
            while (!done)
            {
                foreach (SpaceModel adjacentSpace in currentnode.Space.GetAdjacentSpaces())
                {
                    if (adjacentSpace != null)
                    {
                        if (adjacentSpace.GetNode() == null)
                        {
                            // Is null, need new node
                            int newNodeCost = currentnode.Cost + costDeterminer.GetMovementCost(adjacentSpace);
                            PathfindingNode newNode = new PathfindingNode(adjacentSpace, currentnode, newNodeCost, false, destSpace);
                            allNodes.Add(newNode);
                            adjacentSpace.SetNode(newNode);
                        }
                        else
                        {
                            // not null, there is a node here
                            PathfindingNode nextNode = adjacentSpace.GetNode();
                            int newNodeCost = currentnode.Cost + costDeterminer.GetMovementCost(adjacentSpace);
                            double newPathfindingCost = newNodeCost;
                            if (!nextNode.Seen)
                            {
                                // Next node hasn't been visited yet
                                //nextNode.Update(newNodeCost, newPathfindingCost, currentnode);
                                nextNode.Update(newNodeCost, currentnode);
                            }

                        }
                    }
                }
                currentnode.SetSeen();
                if (!currentnode.Space.Equals(destSpace))
                {
                    PathfindingNode lowestNode = null;
                    foreach (PathfindingNode node in allNodes)
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

            List<SpaceModel> path = new List<SpaceModel>();
            PathfindingNode backtrackNode = currentnode;
            while (backtrackNode != null)
            {
                path.Add(backtrackNode.Space);
                backtrackNode = backtrackNode.GetParent();
            }
            path.Reverse();

            foreach (PathfindingNode node in allNodes)
            {
                node.Space.SetNode(null);

                //node.GetSpace().ClearHighlighted();
            }

            path.Remove(startSpace);
            return path;
        }
    }
}
