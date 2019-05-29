using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathfindingNode
{
    private int cost;
    private double pathfindingCost;
    private bool seen;
    private SpaceModel thisSpace;

    // A* only variable. Guess of remaining distance
    private int ASRemaining;
    private PathfindingNode parent;

    // Create a node on a space. Special constructor for A* nodes, includes guess of remaining distance
    public PathfindingNode(SpaceModel space, PathfindingNode parent, int cost, double pathfindingCost, bool seen, SpaceModel destination)
    {
        thisSpace = space;
        this.cost = cost;
        this.parent = parent;
        this.seen = seen;
        this.pathfindingCost = pathfindingCost;

        if (destination != null)
        {
            // Spaces Up/Down (UD) +
            // Spaces Side - (UD) / 2 (Min 0)
            // This section gets the minimum distance between two points on our hex map.
            var coords = space.GetDoubledCoords();
            int vertical = Math.Abs(coords.row - coords.row);
            int furtherHorizontal = Math.Max(Math.Abs(coords.col - coords.col) - vertical, 0) / 2;
            ASRemaining = vertical + furtherHorizontal;
        }
    }

    // A* needs a guess of how far this space is from the destination
    public double GetASCost()
    {
        return pathfindingCost + ASRemaining;
    }
    public PathfindingNode GetParent()
    {
        return parent;
    }

    // has this node been visited by the pathfinder
    public bool BeenSeen()
    {
        return seen;
    }

    // This node has been visited
    public void Seen()
    {
        seen = true;
    }

    // Gets the space this node is attached to.
    public SpaceModel GetSpace()
    {
        return thisSpace;
    }

    // Gets the cost to travel to this node.
    public int GetCost()
    {
        return cost;
    }

    public double GetPathfindingCost()
    {
        return pathfindingCost;
    }

    public int GetRemainingCost()
    {
        return ASRemaining;
    }

    // recursive function
    public List<PathfindingNode> GetPath(bool endPoint)
    {
        if(parent == null)
        {
            return new List<PathfindingNode>();
        }
        else if(endPoint)
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

    internal void Update(int newCost, double newPathfindingCost, PathfindingNode newParent)
    {
        if (newPathfindingCost < pathfindingCost)
        {
            cost = newCost;
            pathfindingCost = newPathfindingCost;
            parent = newParent;
        }
    }
}
