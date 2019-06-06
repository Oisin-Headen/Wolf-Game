using System;public class WolfPlayer : AbstractPlayer{    public WolfPlayer(GameModel gameModel) : base(Player.Wolves, gameModel)
    {        // TODO Remove Test Unit        var space = gameModel.map.GetSpace(new DoubledCoords(40, 128));        units.Add(UnitFactory.CreateWolf(space, this, gameModel));
    }
    // At the start of the wolf player's turn.
    public override void StartTurn()
    {
        foreach(var unit in units)
        {
            unit.StartTurn();
        }
    }
}