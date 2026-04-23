using UnityEngine;

public class PatrolState : IState
{
    Hunter hunter;

    public PatrolState(Hunter h) => hunter = h;

    public void Enter() { }

    public void Update()
    {
        Transform wp = hunter.waypoints[hunter.currentWP];

        hunter.Movement.Move(wp.position - hunter.transform.position);

        if (Vector3.Distance(hunter.transform.position, wp.position) < 1f)
            hunter.currentWP = (hunter.currentWP + 1) % hunter.waypoints.Length;

        var target = hunter.Perception.GetTarget();

        if (target != null)
        {
            hunter.ChangeState(hunter.huntState);
            return;
        }

        if (hunter.energy <= 0)
        {
            hunter.ChangeState(hunter.idleState);
        }
    }

    public void Exit() { }
}
