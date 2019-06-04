using System.Collections;
using System.Collections.Generic;

public abstract class AbstractPlayer
{
    public List<UnitModel> units;
    public readonly Player thisplayer;
    protected GameController gameController;

    protected AbstractPlayer(Player newplayer, GameController gameController)
    {
        this.gameController = gameController;
        thisplayer = newplayer;
        units = new List<UnitModel>();
    }
    public abstract void EndTurn();
    public abstract void StartTurn();
}
