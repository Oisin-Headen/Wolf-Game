using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{
    public static UnitModel CreateWolf(SpaceModel space, AbstractPlayer player, GameModel gameModel)
    {
        return new UnitModel(
            5, // attack
            5, // defence
            5, // Max HP
            3, // movement
            1, // heal amount
            3, // Vision range
            space, 
            player, 
            gameModel, 
            new IgnoreForestLOS()
        );
    }
}
