using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class MapModel
    {
        // Indexes must add to an even number
        readonly SpaceModel[][] map;
        readonly List<SpaceModel> allSpaces;

        public MapModel(GameModel gameModel)
        {
            allSpaces = new List<SpaceModel>();
            map = new SpaceModel[Utilities.MAP_HEIGHT][];
            for (int row = 0; row < Utilities.MAP_HEIGHT; ++row)
            {
                map[row] = new SpaceModel[Utilities.MAP_WIDTH * 2];

                int col = 0;
                if (row % 2 != 0)
                {
                    ++col;
                }
                for (; col < Utilities.MAP_WIDTH * 2; col += 2)
                {
                    map[row][col] = new SpaceModel(row, col, gameModel, this);
                    allSpaces.Add(map[row][col]);
                }
            }

            foreach (var row in map)
            {
                foreach (var space in row)
                {
                    if (space != null)
                    {
                        space.SetAdjacentSpaces();
                    }
                }
            }


            var ocean = GetOceanSpaces();

            foreach(var space in ocean)
            {
                space.Ocean = true;
            }

            // River Generation
            var rand = new System.Random();
            for (int i = 0; i < 10; i++)
            {
                
                int fifthHeight, fifthWidth, randHeight, randWidth;
                fifthHeight = Utilities.MAP_HEIGHT / 3;
                fifthWidth = Utilities.MAP_WIDTH * 2 / 3;

                randHeight = rand.Next(fifthHeight);
                randWidth = rand.Next(fifthWidth);

                randHeight += (fifthHeight);
                randWidth +=  (fifthWidth);

                if ((randHeight + randWidth) % 2 != 0)
                {
                    randHeight++;
                }

                List<SpaceModel> river = new List<SpaceModel>();

                var origin = map[randHeight][randWidth];
                origin.SetTerrain(new SpaceTerrain(SpaceTerrain.SpaceElevation.Mountain,
                    SpaceTerrain.SpaceBaseTerrain.None,
                    SpaceTerrain.SpaceFeature.Frosted));



                RiverDirection direction = null;
                var adjSpaces = origin.GetAdjacentSpaces();
                for(int index = 0; index < adjSpaces.Length; index++)
                {
                    if(direction == null)
                    {
                        direction = new RiverDirection(index);
                    }
                    else if(adjSpaces[direction.GetDirectionIndex()].DistCenter() < adjSpaces[index].DistCenter())
                    {
                        direction = new RiverDirection(index);
                    }
                }

                SpaceModel next = adjSpaces[direction.GetDirectionIndex()];

                bool done = false;
                while (!next.Ocean)
                {
                    river.Add(next);
                    var nextSpaces = next.GetAdjacentSpaces();

                    int directionChangeChance = rand.Next(100);

                    int changeThreshold = 20;
                    try
                    {
                        if (nextSpaces[direction.GetDirectionIndex()].Terrain.elevation == SpaceTerrain.SpaceElevation.Mountain ||
                           nextSpaces[direction.GetDirectionIndex()].Terrain.elevation == SpaceTerrain.SpaceElevation.Hill)
                        {
                            changeThreshold = 50;
                        }

                        // Could randomly change direction if the side spaces aren't mountains or hills.
                        if (directionChangeChance <= changeThreshold && (
                            nextSpaces[direction.GetAbove()].Terrain.elevation != SpaceTerrain.SpaceElevation.Hill ||
                            nextSpaces[direction.GetAbove()].Terrain.elevation != SpaceTerrain.SpaceElevation.Mountain))
                        {
                            direction.Increase();
                        }
                        else if (directionChangeChance <= changeThreshold * 2 && (
                            nextSpaces[direction.GetBelow()].Terrain.elevation != SpaceTerrain.SpaceElevation.Hill ||
                            nextSpaces[direction.GetBelow()].Terrain.elevation != SpaceTerrain.SpaceElevation.Mountain))
                        {
                            direction.Decrease();
                        }

                        next = nextSpaces[direction.GetDirectionIndex()];
                    }
                    catch(IndexOutOfRangeException)
                    {
                        done = true;
                    }
                }

                foreach (var riverSpace in river)
                {
                    riverSpace.SetTerrain(new SpaceTerrain(SpaceTerrain.SpaceElevation.Water,
                        SpaceTerrain.SpaceBaseTerrain.None,
                        SpaceTerrain.SpaceFeature.None));
                }
            }
        }

        public List<SpaceModel> GetDeepForests()
        {
            List<SpaceModel> deepforests = new List<SpaceModel>();
            foreach (var space in allSpaces)
            { 
                if (space != null)
                {
                    if (space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                    {
                        deepforests.Add(space);
                    }
                }
            }
            return deepforests;
        }

        public List<SpaceModel> GetWaterSpaces()
        {
            List<SpaceModel> waterSpaces = new List<SpaceModel>();
            foreach(var space in allSpaces)
            {
                if (space != null)
                {
                    if (space.Terrain.elevation == SpaceTerrain.SpaceElevation.Water)
                    {
                        waterSpaces.Add(space);
                    }
                }
            }
            return waterSpaces;
        }

        public List<SpaceModel> GetOceanSpaces()
        {
            var origin = map[0][0];
            if(origin.Terrain.elevation != SpaceTerrain.SpaceElevation.Water)
            {
                return null;
            }
            return Pathfinding.PathfindingDijkstras.GetWaterBody(origin);
        }

        // Movement Methods. Only used in Space generation.
        public SpaceModel GetNE(DoubledCoords coords)
        {
            int newSpaceRow = coords.row - 1;
            int newSpaceColumn = coords.col + 1;
            if (newSpaceRow < 0 || newSpaceColumn >= Utilities.MAP_WIDTH * 2)
            {
                return null;
            }
            return map[newSpaceRow][newSpaceColumn];
        }

        public SpaceModel GetE(DoubledCoords coords)
        {
            int newSpaceRow = coords.row;
            int newSpaceColumn = coords.col + 2;
            if (newSpaceColumn >= Utilities.MAP_WIDTH * 2)
            {
                return null;
            }
            return map[newSpaceRow][newSpaceColumn];
        }

        public SpaceModel GetSE(DoubledCoords coords)
        {
            int newSpaceRow = coords.row + 1;
            int newSpaceColumn = coords.col + 1;
            if (newSpaceRow >= Utilities.MAP_HEIGHT || newSpaceColumn >= Utilities.MAP_WIDTH * 2)
            {
                return null;
            }
            return map[newSpaceRow][newSpaceColumn];
        }
        public SpaceModel GetSW(DoubledCoords coords)
        {
            int newSpaceRow = coords.row + 1;
            int newSpaceColumn = coords.col - 1;
            if (newSpaceRow >= Utilities.MAP_HEIGHT || newSpaceColumn < 0)
            {
                return null;
            }
            return map[newSpaceRow][newSpaceColumn];
        }
        public SpaceModel GetW(DoubledCoords coords)
        {
            int newSpaceRow = coords.row;
            int newSpaceColumn = coords.col - 2;
            if (newSpaceColumn < 0)
            {
                return null;
            }
            return map[newSpaceRow][newSpaceColumn];
        }
        public SpaceModel GetNW(DoubledCoords coords)
        {
            int newSpaceRow = coords.row - 1;
            int newSpaceColumn = coords.col - 1;
            if (newSpaceRow < 0 || newSpaceColumn < 0)
            {
                return null;
            }
            return map[newSpaceRow][newSpaceColumn];
        }

        public SpaceModel GetSpace(DoubledCoords normalCoord)
        {
            if (normalCoord == null)
            {
                return null;
            }
            try
            {
                return map[normalCoord.row][normalCoord.col];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        //todo test method
        public void ExploreAll()
        {
            foreach (var row in map)
            {
                foreach (var col in row)
                {
                    if(col!=null)
                    {
                        col.Explore();
                    }
                }
            }
        }
    }
}
