using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class MapModel
    {
        readonly SpaceModel[][] map;
        public MapModel(GameModel gameModel)
        {
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
        }

        public List<SpaceModel> GetDeepForests()
        {
            List<SpaceModel> deepforests = new List<SpaceModel>();
            foreach (var row in map)
            {
                foreach (var space in row)
                {
                    if (space != null)
                    {
                        if (space.Terrain.feature == SpaceTerrain.SpaceFeature.Deep_Forest)
                        {
                            deepforests.Add(space);
                        }
                    }
                }
            }
            return deepforests;
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
                Debug.Log("{" + normalCoord.row + ", " + normalCoord.col + "]");
                return null;
            }
        }
    }
}