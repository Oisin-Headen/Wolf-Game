using System;

public class PreEndTurnTask
{
    private UnitModel unit;

    public PreEndTurnTask(UnitModel unit)
    {
        this.unit = unit;
        unit.MovementTask = this;
    }

    public bool Complete { get; internal set; }

    internal void Show()
    {
        unit.Space.Clicked();
    }
}