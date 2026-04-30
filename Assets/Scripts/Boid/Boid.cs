using UnityEngine;
[RequireComponent(typeof(SteeringBehaviors))]
[RequireComponent(typeof(BoidFlocking))]
public class Boid : MonoBehaviour
{
    [Header("Detection Ranges")]
    [SerializeField] private float foodDetectionRange = 8f;
    [SerializeField] private float hunterDetectionRange = 10f;

    [Header("Food")]
    [SerializeField] private float consumeDistance = 1.5f;

    [Header("Wander")]
    [SerializeField] private float wanderRadius = 2f;
    [SerializeField] private float wanderDistance = 3f;

    [Header("Layers")]
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private LayerMask hunterLayer;

    [Header("Debug Colors")]
    private Color foodRangeColor = Color.yellow;
    private Color hunterRangeColor = Color.magenta;
    private Color consumeRangeColor = Color.cyan;

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
        Hunter hunter = FindHunter();
        if (hunter != null)
        {
            Vector3 steeringForce = steering.Evade(hunter.transform,hunter.GetVelocity());
            steering.Move(steeringForce);
            return;
        }
        Food nearestFood = FindNearestFood();
        if (nearestFood != null)
        {
            Vector3 steeringForce = steering.Arrive(nearestFood.transform.position);
            steering.Move(steeringForce);
            float distanceToFood = Vector3.Distance(transform.position, nearestFood.transform.position);
            if (distanceToFood <= consumeDistance)
            {
                nearestFood.Consume();
            }
            return;
        }
        if (flocking.HasNearbyBoids())
        {
            Debug.Log("Entro");
            Vector3 steeringForce = flocking.CalculateFlocking();
            steering.Move(steeringForce);
            return;
        }
        Vector3 wanderForce = steering.Wander(wanderRadius,wanderDistance);
        steering.Move(wanderForce);
    }
    private Food FindNearestFood()
    {
        Collider[] foods = Physics.OverlapSphere(transform.position,foodDetectionRange,foodLayer);
        Food nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider foodCollider in foods)
        {
            Food food = foodCollider.GetComponent<Food>();
            if (food == null || !food.gameObject.activeInHierarchy)
                continue;
            float distance = Vector3.Distance(transform.position,food.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = food;
            }
        }
        return nearest;
    }
    private Hunter FindHunter()
    {
        Collider[] hunters = Physics.OverlapSphere(transform.position,hunterDetectionRange,hunterLayer);
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
        Gizmos.DrawWireSphere(transform.position,foodDetectionRange);
        Gizmos.color = foodRangeColor;
        Gizmos.color = hunterRangeColor;
        Gizmos.DrawWireSphere(transform.position, hunterDetectionRange);
        Gizmos.color = consumeRangeColor;
        Gizmos.DrawWireSphere(transform.position,consumeDistance);
    }
}