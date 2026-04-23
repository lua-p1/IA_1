using System.Collections.Generic;
using UnityEngine;

public class BoidDecisionTree : IDecisionNode
{
    public ISteeringBehavior Decide(Boid boid)
    {
        var food = boid.Perception.GetFood();

        if (food != null)
            return new ArriveBehavior(food.transform, boid.slowRadius);

        var hunter = boid.Perception.GetHunter();

        if (hunter != null)
            return new EvadeBehavior(hunter.transform);

        var neighbors = boid.Perception.GetNeighbors();

        if (neighbors.Count > 0)
            return new FlockingBehavior(neighbors);

        return new WanderBehavior();
    }
}
