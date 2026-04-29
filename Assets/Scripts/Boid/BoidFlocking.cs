using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehaviors))]
public class BoidFlocking : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float separationRadius = 2f;
    [SerializeField] private float alignmentRadius = 4f;
    [SerializeField] private float cohesionRadius = 5f;

    [Header("Weights")]
    [SerializeField] private float separationWeight = 2f;
    [SerializeField] private float alignmentWeight = 1f;
    [SerializeField] private float cohesionWeight = 0.5f;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask boidLayer;

    private SteeringBehaviors steering;

    private void Awake()
    {
        steering = GetComponent<SteeringBehaviors>();
    }

    public Vector3 CalculateFlocking()
    {
        Vector3 separation = Separation() * separationWeight;
        Vector3 alignment = Alignment() * alignmentWeight;
        Vector3 cohesion = Cohesion() * cohesionWeight;

        return separation + alignment + cohesion;
    }

    private List<Boid> GetNearbyBoids(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, boidLayer);

        List<Boid> nearby = new List<Boid>();

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            Boid boid = hit.GetComponent<Boid>();

            if (boid != null)
                nearby.Add(boid);
        }

        return nearby;
    }

    private Vector3 Separation()
    {
        List<Boid> neighbors = GetNearbyBoids(separationRadius);
        var desired = Vector3.zero;
        if (neighbors.Count == 0)
            return Vector3.zero;
        Vector3 force = Vector3.zero;
        foreach (Boid boid in neighbors)
        {
            Vector3 diretions = transform.position - boid.transform.position;
            desired += diretions;
        }
        if (desired == Vector3.zero)
            return Vector3.zero;
        desired.Normalize();
        desired *= steering.GetMaxSpeed();
        var steer = desired - steering.Velocity;
        steer = Vector3.ClampMagnitude(steer, steering.GetMaxForce());
        return steer;
    }

    private Vector3 Alignment()
    {
        List<Boid> neighbors = GetNearbyBoids(alignmentRadius);
        if (neighbors.Count == 0)
            return Vector3.zero;
        var desired = Vector3.zero;
        foreach (Boid boid in neighbors)
        {
            SteeringBehaviors otherSteering = boid.GetComponent<SteeringBehaviors>();
            desired += otherSteering.Velocity;
        }
        if (desired == Vector3.zero)
            return Vector3.zero;
        desired.Normalize();
        desired *= steering.GetMaxSpeed();
        var steer = desired - steering.Velocity;
        steer = Vector3.ClampMagnitude(steer, steering.GetMaxForce());
        return steer;
    }

    private Vector3 Cohesion()
    {
        List<Boid> neighbors = GetNearbyBoids(cohesionRadius);
        if (neighbors.Count == 0)
            return Vector3.zero;
        Vector3 center = Vector3.zero;
        foreach (Boid boid in neighbors)
            center += boid.transform.position;
        center /= neighbors.Count;
        var dir = center - this.transform.position;
        var desired = dir.normalized;
        desired *= steering.GetMaxSpeed();
        var steer = desired - steering.Velocity;
        steer = Vector3.ClampMagnitude(steer, steering.GetMaxForce());
        return steer;
    }

    public bool HasNearbyBoids()
    {
        return GetNearbyBoids(cohesionRadius).Count > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alignmentRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, cohesionRadius);
    }
}