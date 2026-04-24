using UnityEngine;

public class WanderBehavior : ISteeringBehavior
{
    public Vector3 Calculate(Boid boid)
    {
        return Random.insideUnitSphere * boid.maxSpeed;
    }
}