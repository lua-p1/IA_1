using UnityEngine;

public class Hunter : MonoBehaviour
{
    public float maxSpeed = 6;
    public float energy = 100;
    public float energyDrain = 10;
    public float energyRecovery = 15;

    public Transform[] waypoints;
    public int currentWP = 0;

    public float visionRange = 15;

    public HunterMovement Movement { get; private set; }
    public HunterPerception Perception { get; private set; }

    StateMachine fsm;

    public IdleState idleState;
    public PatrolState patrolState;
    public HuntState huntState;

    void Start()
    {
        Movement = new HunterMovement(this);
        Perception = new HunterPerception(this);

        fsm = new StateMachine();

        idleState = new IdleState(this);
        patrolState = new PatrolState(this);
        huntState = new HuntState(this);

        fsm.ChangeState(patrolState);
    }

    void Update()
    {
        fsm.Update();
    }

    public void ChangeState(IState state)
    {
        fsm.ChangeState(state);
    }
}