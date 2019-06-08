using System.Collections.Generic;public class WolfPlayer : AbstractPlayer{    private List<SpaceModel> deepForests;    public WolfPlayer(GameModel gameModel, List<SpaceModel> deepForests) : base(Player.Wolves, gameModel)
    {
        this.deepForests = deepForests;        foreach(var deepforest in deepForests)
        {
            deepforest.Explore();            foreach(var adjacentSpace in deepforest.GetAdjacentSpaces())
            {                if(adjacentSpace != null)
                {
                    adjacentSpace.Explore();
                }             }
        }
        // TODO Remove Test Unit        if(deepForests.Count > 3)
        {
            var space = deepForests[0];
            units.Add(UnitFactory.CreateWolf(space, this, gameModel));            space = deepForests[1];            units.Add(UnitFactory.CreateWolf(space, this, gameModel));            space = deepForests[2];            units.Add(UnitFactory.CreateWolf(space, this, gameModel));            foreach (var unit in units)
            {
                unit.Explore();
            }
        }
    }
    //// At the start of the wolf player's turn.
    //public override void StartTurn()
    //{
    //    foreach(var unit in units)
    //    {
    //        unit.StartTurn();
    //    }
    //}
}