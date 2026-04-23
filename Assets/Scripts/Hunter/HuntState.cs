using UnityEngine;

public class HuntState : IState
{
    Hunter hunter;
    Boid target;

    public HuntState(Hunter h) => hunter = h;

    public void Enter()
    {
        target = hunter.Perception.GetTarget();
    }

    public void Update()
    {
        if (target == null)
        {
            hunter.ChangeState(hunter.patrolState);
            return;
        }

        Vector3 futurePos = target.transform.position + target.transform.forward * 2f;
        hunter.Movement.Move(futurePos - hunter.transform.position);
        hunter.energy -= hunter.energyDrain * Time.deltaTime;

        if (hunter.Perception.GetTarget() == null)
        {
            hunter.ChangeState(hunter.patrolState);
            return;
        }

        if (hunter.energy <= 0)
        {
            hunter.ChangeState(hunter.idleState);
        }
    }

    public void Exit() { }
}
