using UnityEngine;

[RequireComponent(typeof(SteeringBehaviors))]
[RequireComponent(typeof(BoidFlocking))]
public class Boid : MonoBehaviour
{
    [Header("Detection Ranges")]
    [SerializeField] private float foodDetectionRange = 8f;
    [SerializeField] private float hunterDetectionRange = 10f;

    [Header("Wander")]
    [SerializeField] private float wanderRadius = 2f;
    [SerializeField] private float wanderDistance = 3f;

    [Header("Layers")]
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private LayerMask hunterLayer;

    [Header("Debug Colors")]
    [SerializeField] private Color foodRangeColor = Color.yellow;
    [SerializeField] private Color hunterRangeColor = Color.magenta;

    private SteeringBehaviors steering;
    private BoidFlocking flocking;

    private void Awake()
    {
        steering = GetComponent<SteeringBehaviors>();
        flocking = GetComponent<BoidFlocking>();
    }

    private void Update()
    {
        MakeDecision();
    }

    private void MakeDecision()
    {
        Food nearestFood = FindNearestFood();

        // PRIORIDAD 1: Buscar comida
        if (nearestFood != null)
        {
            Vector3 steeringForce = steering.Arrive(nearestFood.transform.position);
            steering.Move(steeringForce);

            if (Vector3.Distance(transform.position, nearestFood.transform.position) < 1f)
            {
                nearestFood.Consume();
            }

            return;
        }

        // PRIORIDAD 2: Huir del hunter
        Hunter hunter = FindHunter();

        if (hunter != null)
        {
            Vector3 steeringForce = steering.Evade(
                hunter.transform,
                hunter.GetVelocity()
            );

            steering.Move(steeringForce);
            return;
        }

        // PRIORIDAD 3: Flocking
        if (flocking.HasNearbyBoids())
        {
            Vector3 steeringForce = flocking.CalculateFlocking();
            steering.Move(steeringForce);
            return;
        }

        // PRIORIDAD 4: Wander
        Vector3 wanderForce = steering.Wander(wanderRadius, wanderDistance);
        steering.Move(wanderForce);
    }

    private Food FindNearestFood()
    {
        Collider[] foods = Physics.OverlapSphere(
            transform.position,
            foodDetectionRange,
            foodLayer
        );

        Food nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider foodCollider in foods)
        {
            Food food = foodCollider.GetComponent<Food>();

            if (food == null || !food.gameObject.activeInHierarchy)
                continue;

            float dist = Vector3.Distance(
                transform.position,
                food.transform.position
            );

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = food;
            }
        }

        return nearest;
    }

    private Hunter FindHunter()
    {
        Collider[] hunters = Physics.OverlapSphere(
            transform.position,
            hunterDetectionRange,
            hunterLayer
        );

        if (hunters.Length == 0)
            return null;

        return hunters[0].GetComponent<Hunter>();
    }

    public Vector3 GetVelocity()
    {
        return steering.Velocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = foodRangeColor;
        Gizmos.DrawWireSphere(transform.position, foodDetectionRange);

        Gizmos.color = hunterRangeColor;
        Gizmos.DrawWireSphere(transform.position, hunterDetectionRange);
    }
}
