using UnityEngine;
public interface ISteeringBehavior
{
    Vector3 Calculate(Boid boid);
}