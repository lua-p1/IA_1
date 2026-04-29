public class IdleState : HunterState
{
    private float timer;

    public IdleState(Hunter hunter) : base(hunter) { }

    public override void Enter()
    {
        timer = hunter.RestDuration;
    }

    public override void Update()
    {
        timer -= UnityEngine.Time.deltaTime;

        if (timer <= 0f)
        {
            hunter.RestoreEnergy();
            hunter.ChangeState(new PatrolState(hunter));
        }
    }

    public override void Exit()
    {
    }
}