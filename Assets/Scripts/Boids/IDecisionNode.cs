using UnityEngine;

public interface IDecisionNode
{
    ISteeringBehavior Decide(Boid boid);
}

