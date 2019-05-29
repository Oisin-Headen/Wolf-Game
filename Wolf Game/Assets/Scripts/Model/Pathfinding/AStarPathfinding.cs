using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfinding
{
    public static List<SpaceModel> GetPathToDestination(SpaceModel startSpace, SpaceModel destSpace)
    {
        List<PathfindingNode> allNodes = new List<PathfindingNode>();

        PathfindingNode currentnode = new PathfindingNode(startSpace, null, 0 , 0, true, destSpace);
        allNodes.Add(currentnode);
        bool done = false;
        while (!done)
        {
            foreach (SpaceModel adjacentSpace in currentnode.GetSpace().GetAdjacentSpaces())
            {
                if (adjacentSpace != null)
                {
                    if (adjacentSpace.GetNode() == null)
                    {
                        // Is null, need new node
                        int newNodeCost = currentnode.GetCost() + adjacentSpace.GetMovementCost();
                        double newPathfindingCost = newNodeCost;
                        if(adjacentSpace == destSpace)
                        {
                            newPathfindingCost = 1;
                        }
                        PathfindingNode newNode = new PathfindingNode(adjacentSpace, currentnode, newNodeCost, newPathfindingCost, false, destSpace);
                        allNodes.Add(newNode);
                        adjacentSpace.SetNode(newNode);
                    }
                    else
                    {
                        // not null, there is a node here
                        PathfindingNode nextNode = adjacentSpace.GetNode();
                        int newNodeCost = currentnode.GetCost() + adjacentSpace.GetMovementCost();
                        double newPathfindingCost = newNodeCost;
                        if (!nextNode.BeenSeen())
                        {
                            // Next node hasn't been visited yet
                            nextNode.Update(newNodeCost, newPathfindingCost, currentnode);
                        }

                    }
                }
            }
            currentnode.Seen();
            if (!currentnode.GetSpace().Equals(destSpace))
            {
                PathfindingNode lowestNode = null;
                foreach (PathfindingNode node in allNodes)
                {
                    if (!node.BeenSeen())
                    {
                        if (lowestNode == null)
                        {
                            lowestNode = node;
                        }
                        else
                        {
                            if (node.GetASCost() <= lowestNode.GetASCost())
                            {
                                if (node.GetRemainingCost() <= lowestNode.GetRemainingCost())
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
        while(backtrackNode != null)
        {
            path.Add(backtrackNode.GetSpace());
            backtrackNode = backtrackNode.GetParent();
        }
        path.Reverse();

        foreach (PathfindingNode node in allNodes)
        {
            node.GetSpace().SetNode(null);

            //node.GetSpace().ClearHighlighted();
        }

        path.Remove(startSpace);
        return path;
    }
}
