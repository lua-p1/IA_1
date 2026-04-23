using UnityEngine;

public class IdleState : IState
{
    Hunter hunter;
    float timer;

    public IdleState(Hunter h) => hunter = h;

    public void Enter()
    {
        timer = 0;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        hunter.energy += hunter.energyRecovery * Time.deltaTime;

        if (timer > 3f)
        {
            hunter.ChangeState(hunter.patrolState);
        }
    }

    public void Exit() { }
}