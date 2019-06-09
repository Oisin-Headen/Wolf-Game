using System;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractPlayer
{
    public List<UnitModel> units;
    public readonly Player thisplayer;
    protected GameModel gameModel;

    private List<IPreEndTurnTask> tasks;

    protected AbstractPlayer(Player newplayer, GameModel gameModel)
    {
        this.gameModel = gameModel;
        thisplayer = newplayer;
        units = new List<UnitModel>();
    }
    //public abstract void EndTurn();
    public void StartTurn()
    {
        tasks = new List<IPreEndTurnTask>();
        foreach (var unit in units)
        {
            unit.StartTurn();
            tasks.Add(new PreEndTurnMovementTask(unit));
        }
    }

    // See if there are any tasks the player needs to complete before ending their turn
    public bool CanEndTurn()
    {
        bool canEndTurn = true;
        foreach(var task in tasks)
        {
            canEndTurn &= task.Complete();
        }
        return canEndTurn;
    }

    // Display the first task that hasn't been completed
    public void PreEndTurnTask()
    {
        bool foundTask = false;
        foreach (var task in tasks)
        {
            if(!foundTask && !task.Complete())
            {
                task.Show();
                foundTask = true;
            }
        }
    }
}
