using UnityEngine;

public class EvadeBehavior : ISteeringBehavior
{
    Transform hunter;

    public EvadeBehavior(Transform hunter)
    {
        this.hunter = hunter;
    }

    public Vector3 Calculate(Boid boid)
    {
        Vector3 future = hunter.position + hunter.forward * 2f;
        return (boid.transform.position - future).normalized * boid.maxSpeed;
    }
}