using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Model
{
    internal static class UnitFactory
    {
        internal static UnitModel CreateWolf(SpaceModel space, APlayer player, GameModel gameModel)
        {
            return new UnitModel(
                UnitTypeOverseerSingleton.GetInstance().Wolf, 
                space,
                player,
                gameModel
            );
        }

        internal static UnitModel CreateBattleSpider(SpaceModel space, APlayer player, GameModel gameModel)
        {
            return new UnitModel(
                UnitTypeOverseerSingleton.GetInstance().BattleSpider,
                space,
                player,
                gameModel
            );
        }

        internal static UnitModel CreateWorkerSpider(SpaceModel space, APlayer player, GameModel gameModel)
        {
            return new UnitModel(
                UnitTypeOverseerSingleton.GetInstance().WorkerSpider,
                space,
                player,
                gameModel
            );
        }
    }
}