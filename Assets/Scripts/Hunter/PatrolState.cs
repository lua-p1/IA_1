public class PatrolState : HunterState
{
    public PatrolState(Hunter hunter) : base(hunter) { }

    public override void Enter()
    {
    }

    public override void Update()
    {
        hunter.Patrol();

        hunter.ConsumeEnergy(hunter.PatrolCost);

        if (!hunter.HasEnergy())
        {
            hunter.ChangeState(new IdleState(hunter));
            return;
        }

        if (hunter.CanSeeBoid())
        {
            hunter.ChangeState(new HuntingState(hunter));
        }
    }

    public override void Exit()
    {
    }
}