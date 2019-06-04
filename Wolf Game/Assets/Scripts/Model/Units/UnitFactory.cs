using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{
    public static UnitModel CreateWolf(SpaceModel space, AbstractPlayer player, GameController gameController)
    {
        return new UnitModel(5, 5, 5, 3, 1, 2, space, player, gameController);
    }
}
