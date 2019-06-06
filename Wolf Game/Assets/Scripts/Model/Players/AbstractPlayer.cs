using System.Collections;
using System.Collections.Generic;

public abstract class AbstractPlayer
{
    public List<UnitModel> units;
    public readonly Player thisplayer;
    protected GameModel gameModel;

    protected AbstractPlayer(Player newplayer, GameModel gameModel)
    {
        this.gameModel = gameModel;
        thisplayer = newplayer;
        units = new List<UnitModel>();
    }
    //public abstract void EndTurn();
    public abstract void StartTurn();
}
