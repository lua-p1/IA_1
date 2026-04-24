using UnityEngine;

public class ArriveBehavior : ISteeringBehavior
{
    Transform target;
    float slowRadius;

    public ArriveBehavior(Transform target, float slowRadius)
    {
        this.target = target;
        this.slowRadius = slowRadius;
    }

    public Vector3 Calculate(Boid boid)
    {
        Vector3 desired = target.position - boid.transform.position;
        float dist = desired.magnitude;

        float speed = boid.maxSpeed;

        if (dist < slowRadius)
            speed *= dist / slowRadius;

        return desired.normalized * speed;
    }
}