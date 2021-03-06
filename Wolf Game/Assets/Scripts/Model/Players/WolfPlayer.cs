﻿using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Model
{
    public class WolfPlayer : APlayer
    {
        private List<SpaceModel> deepForests;

        public WolfPlayer(GameModel gameModel, List<SpaceModel> deepForests) : base(PlayerType.Wolves, gameModel)
        {
            this.deepForests = deepForests;
            foreach (var deepforest in deepForests)
            {
                deepforest.Explore();
                foreach (var adjacentSpace in deepforest.GetAdjacentSpaces())
                {
                    if (adjacentSpace != null)
                    {
                        adjacentSpace.Explore();
                    }
                }
            }
            // TODO Remove Test Unit
            if (deepForests.Count > 4)
            {
                //units.Add(UnitFactory.CreateWolf(deepForests[0], this, gameModel));
                //units.Add(UnitFactory.CreateWolf(deepForests[1], this, gameModel));
                //units.Add(UnitFactory.CreateWolf(deepForests[2], this, gameModel));

                var impassibleChecker = new OrdinaryMovementCost();
                for (int i = 0; i < 3; i++)
                {
                    var space = deepForests[i];
                    if (!space.Occupied())
                    {
                        units.Add(UnitFactory.CreateWorkerSpider(space, this, gameModel));
                    }
                    int besideNum = 0;
                    foreach (var besideSpace in space.GetAdjacentSpaces())
                    {
                        if ((!besideSpace.Occupied()) && impassibleChecker.GetMovementCost(besideSpace) != -1)
                        {
                            if (besideNum < 2)
                            {
                                units.Add(UnitFactory.CreateBattleSpider(besideSpace, this, gameModel));
                            }
                            else
                            {
                                units.Add(UnitFactory.CreateWolf(besideSpace, this, gameModel));
                            }
                            besideNum++;
                        }
                    }
                }

                foreach (var space in deepForests[3].GetAdjacentSpaces())
                {
                    if ((!space.Occupied()))
                    {
                        units.Add(UnitFactory.CreateScoutEagle(space, this, gameModel));
                    }
                }

                foreach (var unit in units)
                {
                    unit.Explore();
                }
            }
        }

        //// At the start of the wolf player's turn.
        //public override void StartTurn()
        //{
        //    foreach (var unit in units)
        //    {
        //        unit.StartTurn();
        //    }
        //}
    }
}
