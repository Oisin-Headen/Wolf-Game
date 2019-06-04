using System;public class WolfPlayer : AbstractPlayer{    public WolfPlayer(GameController gameController) : base(Player.Wolves, gameController)
    {        // TODO Remove Test Unit        var space = gameController.map.GetSpace(new DoubledCoords(40, 128));        units.Add(UnitFactory.CreateWolf(space, this, gameController));
    }    public override void EndTurn()
    {
    }

    public override void StartTurn()
    {
        foreach(var unit in units)
        {
            unit.StartTurn();
        }
    }
}