using System.Collections.Generic;
using UnityEngine;

public class FlockingBehavior : ISteeringBehavior
{
    List<Boid> neighbors;

    public FlockingBehavior(List<Boid> neighbors)
    {
        this.neighbors = neighbors;
    }

    public Vector3 Calculate(Boid boid)
    {
        Vector3 sep = Vector3.zero;
        Vector3 ali = Vector3.zero;
        Vector3 coh = Vector3.zero;

        foreach (var b in neighbors)
        {
            sep += (boid.transform.position - b.transform.position);
            ali += b.Velocity;
            coh += b.transform.position;
        }

        ali /= neighbors.Count;
        coh = (coh / neighbors.Count) - boid.transform.position;

        return sep * 2f + ali + coh;
    }
}
