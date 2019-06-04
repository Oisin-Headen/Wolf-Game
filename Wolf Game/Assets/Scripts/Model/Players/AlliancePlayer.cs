

public class AlliancePlayer : AbstractPlayer
{
    public AlliancePlayer(GameController gameController) : base(Player.Alliance, gameController)
    {
    }

    public override void EndTurn()
    {
    }

    public override void StartTurn()
    {
        throw new System.NotImplementedException();
    }
}
