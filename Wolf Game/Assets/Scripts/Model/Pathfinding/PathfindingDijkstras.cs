using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Pathfinding class for players
public static class PathfindingDijkstras
{
    // this uses Dijkstra's Algorithm to get all the spaces a player can visit.
    public static List<PathfindingNode> Dijkstras(SpaceModel startSpace, int maxCost, bool ignoreTerrain)
    {
        List<PathfindingNode> allNodes = new List<PathfindingNode>();
        //List<SpaceModel> shortestPath = new List<SpaceModel>
        //{
        //    startSpace
        //};
        PathfindingNode currentnode = new PathfindingNode(startSpace, null, 0, 0, true, null);

        allNodes.Add(currentnode);

        bool done = false;
        while (!done)
        {
            foreach (SpaceModel adjacentSpace in currentnode.GetSpace().GetAdjacentSpaces())
            {
                if (adjacentSpace != null)
                {
                    PathfindingNode nextNode = adjacentSpace.GetNode();
                    if (nextNode != null)
                    {
                        // not null
                        if (!nextNode.BeenSeen())
                        {
                            // Next node hasn't been visited yet
                            if (ignoreTerrain)
                            {
                                nextNode.Update(currentnode.GetCost() + 1, currentnode.GetCost() + 1, currentnode);
                            }
                            else
                            {
                                int adjacentSpaceCost = currentnode.GetCost() + adjacentSpace.GetMovementCost();
                                double adjacentSpacePathfindingCost = adjacentSpaceCost;

                                nextNode.Update(adjacentSpaceCost, adjacentSpacePathfindingCost, currentnode);
                            }
                        }
                    }
                    else
                    {
                        // Is null, need new node
                        int newNodeCost;
                        double pathfindingCost;

                        if (ignoreTerrain)
                        {
                            newNodeCost = currentnode.GetCost() + 1;
                            pathfindingCost = newNodeCost;
                        }
                        else
                        {
                            newNodeCost = currentnode.GetCost() + adjacentSpace.GetMovementCost();
                            pathfindingCost = newNodeCost;
                            if (adjacentSpace.Occupied())
                            {
                                // TODO Freindly Units don't block movement
                                pathfindingCost += 100;
                            }
                        }

                        if (newNodeCost <= maxCost)
                        {
                            PathfindingNode newNode = new PathfindingNode(adjacentSpace, currentnode, newNodeCost, pathfindingCost, false, null);
                            allNodes.Add(newNode);
                            adjacentSpace.SetNode(newNode);
                        }
                    }
                }
            }
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
                        if (node.GetPathfindingCost() < lowestNode.GetPathfindingCost())
                        {
                            lowestNode = node;
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
                lowestNode.Seen();
                currentnode = lowestNode;
            }
        }
        foreach (PathfindingNode node in allNodes)
        {
            node.GetSpace().SetNode(null);
        }

        return allNodes;
    }

    public static List<PathfindingNode> GetSpacesForMovementDijkstras(SpaceModel startSpace, int maxCost)
    {
        return Dijkstras(startSpace, maxCost, false);
    }


    // TODO I Think I want to try a different FOV method
    //public static List<PathfindingNode> GetFieldOfView(SpaceModel startSpace, int maxDist, MapModel map)
    //{
    //    List<PathfindingNode> nodes = Dijkstras(startSpace, maxDist, true);
    //    List<PathfindingNode> results = new List<PathfindingNode>();
    //    foreach (PathfindingNode node in nodes)
    //    {
    //        List<SpaceModel> line = MapLinedraw(startSpace, node.GetSpace(), map);
    //        line.Remove(startSpace);
    //        line.Remove(node.GetSpace());
    //        bool blocked = false;
    //        foreach (SpaceModel space in line)
    //        {
    //            blocked |= space.BlocksLOS();
    //        }
    //        if (!blocked)
    //        {
    //            results.Add(node);
    //        }
    //    }
    //    return results;
    //}

    // Helper algorithims

    //private static CubeCoords CoordinatesToCubeCoords(SpaceModel space)
    //{
    //    double x = (space.Column - space.Row) / 2;
    //    double z = space.Row;
    //    double y = -x - z;
    //    return new CubeCoord(x, y, z);
    //}

    private static DoubledCoords CubeCoordsToCoordinates(CubeCoords coord)
    {
        int col = (int)Math.Round(2 * coord.x + coord.z);
        int row = (int)Math.Round(coord.z);
        return new DoubledCoords(col, row);
    }

    private static CubeCoords RoundCubeCoords(CubeCoords cubeCoord)
    {
        float newX = Mathf.Round(cubeCoord.x);
        float newY = Mathf.Round(cubeCoord.y);
        float newZ = Mathf.Round(cubeCoord.z);

        float x_diff = Math.Abs(newX - cubeCoord.x);
        float y_diff = Math.Abs(newY - cubeCoord.y);
        float z_diff = Math.Abs(newZ - cubeCoord.z);

        if (x_diff > y_diff && x_diff > z_diff)
        {
            newX = -newY - newZ;
        }
        else if (y_diff > z_diff)
        {
            newY = -newX - newZ;
        }
        else
        {
            newZ = -newX - newY;
        }
        return new CubeCoords(newX, newY, newZ);
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    private static CubeCoords CubeLerp(CubeCoords a, CubeCoords b, float t)
    {
        return new CubeCoords(Lerp(a.x, b.x, t),
                             Lerp(a.y, b.y, t),
                             Lerp(a.z, b.z, t));
    }

    private static List<SpaceModel> MapLinedraw(SpaceModel start, SpaceModel end, MapModel map)
    {
        CubeCoords a = start.GetCubeCoords();
        CubeCoords b = end.GetCubeCoords();


        float N = CubeDistance(a, b);
        List<SpaceModel> results = new List<SpaceModel>();
        if (Math.Abs(N) > Double.Epsilon)
        {
            for (int i = 0; i <= N; i++)
            {
                CubeCoords coord = RoundCubeCoords(CubeLerp(a, b, 1 / N * i));
                DoubledCoords normalCoord = CubeCoordsToCoordinates(coord);
                SpaceModel newSpace;
                newSpace = map.GetSpace(normalCoord);
                if (newSpace != null)
                {
                    results.Add(newSpace);
                }
            }
        }
        return results;
    }

    private static float CubeDistance(CubeCoords a, CubeCoords b)
    {
        return (Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z)) / 2;
    }
}
