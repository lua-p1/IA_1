public class HuntingState : HunterState
{
    public HuntingState(Hunter hunter) : base(hunter) { }

    public override void Enter()
    {
    }

    public override void Update()
    {
        Boid target = hunter.FindClosestBoid();

        if (target == null)
        {
            hunter.ChangeState(new PatrolState(hunter));
            return;
        }

        hunter.Hunt(target);

        hunter.ConsumeEnergy(hunter.HuntingCost);

        if (!hunter.HasEnergy())
        {
            hunter.ChangeState(new IdleState(hunter));
        }
    }

    public override void Exit()
    {
    }
}