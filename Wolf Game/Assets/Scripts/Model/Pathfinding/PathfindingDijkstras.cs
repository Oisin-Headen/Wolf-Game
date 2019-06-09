﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pathfinding
{
    // Pathfinding class for players
    public static class PathfindingDijkstras
    {
        // Pathfinding Constants
        public const int ONE_SPACE = 100;
        public const int HALF_SPACE = 50;
        public const int THIRD_SPACE = 34;
        public const int QUARTER_SPACE = 25;

        // No unit has 100 spaces of Movement
        public const int REST_OF_MOVEMENT = 10000;


        // this uses Dijkstra's Algorithm to get all the spaces a player can visit.
        public static List<PathfindingNode> Dijkstras(SpaceModel startSpace, int maxCost,
            IMovementCost costDeterminer)
        {
            List<PathfindingNode> allNodes = new List<PathfindingNode>();
            //List<SpaceModel> shortestPath = new List<SpaceModel>
            //{
            //    startSpace
            //};
            PathfindingNode currentnode = new PathfindingNode(startSpace, null, 0, true, null);

            allNodes.Add(currentnode);

            bool done = false;
            while (!done)
            {
                foreach (SpaceModel adjacentSpace in currentnode.Space.GetAdjacentSpaces())
                {
                    // todo for debugging, leave commented.
                    //System.Threading.Thread.Sleep(250);

                    // If space exists
                    if (adjacentSpace != null)
                    {
                        PathfindingNode nextNode = adjacentSpace.GetNode();
                        if (nextNode != null)
                        {
                            // not null, there is already a node here
                            if (!nextNode.Seen)
                            {
                                // Next node hasn't been visited yet
                                int adjacentSpaceCost = currentnode.Cost +
                                    costDeterminer.GetMovementCost(adjacentSpace);
                                //nextNode.Update(adjacentSpaceCost, adjacentSpacePathfindingCost,
                                //currentnode);
                                nextNode.Update(adjacentSpaceCost, currentnode);
                            }

                            // todo remove
                            //UnityToolbag.Dispatcher.InvokeAsync(() =>
                            //nextNode.Space.controller.SetText(nextNode.Cost.ToString()));

                        }
                        else
                        {
                            // Is null, need new node
                            int newNodeCost;
                            newNodeCost = currentnode.Cost +
                                costDeterminer.GetMovementCost(adjacentSpace);
                            if (!adjacentSpace.Occupied())
                            {
                                // TODO Freindly Units don't block movement
                                newNodeCost = -1;
                            }

                            // Can't move to impassable or other units
                            if (costDeterminer.GetMovementCost(adjacentSpace) != -1
                                && newNodeCost != -1)
                            {
                                // Can move to a space when you don't have enough movement left
                                if (currentnode.Cost < maxCost)
                                {
                                    PathfindingNode newNode = new PathfindingNode(adjacentSpace,
                                        currentnode, newNodeCost, false, null);
                                    allNodes.Add(newNode);
                                    //adjacentSpace.SetNode(newNode);

                                    // todo remove after debug finished
                                    //UnityToolbag.Dispatcher.InvokeAsync(() =>
                                    //newNode.Space.controller.SetText(newNode.Cost.ToString()))
                                }
                            }
                        }
                    }
                }
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
                            //if (node.GetPathfindingCost() < lowestNode.GetPathfindingCost())
                            if(node.Cost < lowestNode.Cost)
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
                    lowestNode.SetSeen();
                    //UnityToolbag.Dispatcher.InvokeAsync(() =>
                    //lowestNode.GetSpace().controller.SetAttackable());
                    currentnode = lowestNode;
                }
            }
            foreach (PathfindingNode node in allNodes)
            {
                node.Space.SetNode(null);
            }

            return allNodes;
        }

        public static List<PathfindingNode> GetSpacesForMovementDijkstras(SpaceModel startSpace, 
            int maxCost, IMovementCost costDeterminer)
        {
            return Dijkstras(startSpace, maxCost, costDeterminer);
        }


        // TODO I Think I want to try a different FOV method
        public static List<PathfindingNode> GetFieldOfView(SpaceModel startSpace, int maxSpaces,
            MapModel map, IBlockLOS blockLOS)
        {
            List<PathfindingNode> nodes = Dijkstras(startSpace, maxSpaces * ONE_SPACE, 
                new OneCostMovement());
            List<PathfindingNode> results = new List<PathfindingNode>();
            foreach (PathfindingNode node in nodes)
            {
                List<SpaceModel> line = MapLinedraw(startSpace, node.Space, map);
                line.Remove(startSpace);
                line.Remove(node.Space);
                bool blocked = false;
                foreach (SpaceModel space in line)
                {
                    blocked |= blockLOS.BlocksLOS(startSpace.Terrain.elevation, space);
                }
                if (!blocked)
                {
                    results.Add(node);
                }
            }
            return results;
        }

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
            if ((col + row) % 2 == 1)
            {
                col--;
            }
            return new DoubledCoords(row, col);
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
}