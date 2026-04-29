using UnityEngine;

[RequireComponent(typeof(SteeringBehaviors))]
public class Hunter : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float patrolEnergyCostPerSecond = 5f;
    [SerializeField] private float huntingEnergyCostPerSecond = 10f;
    [SerializeField] private float restDuration = 3f;

    [Header("Detection")]
    [SerializeField] private float visionRange = 12f;
    [SerializeField] private LayerMask boidLayer;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private bool loopWaypoints = true;

    [Header("Debug")]
    [SerializeField] private Color visionColor = Color.red;
    [SerializeField] private Color waypointColor = Color.cyan;

    private SteeringBehaviors steering;
    private HunterState currentState;

    private float currentEnergy;
    private int currentWaypointIndex;
    private int patrolDirection = 1;

    public float CurrentEnergy => currentEnergy;
    public float MaxEnergy => maxEnergy;
    public float RestDuration => restDuration;
    public float PatrolCost => patrolEnergyCostPerSecond;
    public float HuntingCost => huntingEnergyCostPerSecond;

    private void Awake()
    {
        steering = GetComponent<SteeringBehaviors>();
        currentEnergy = maxEnergy;
    }

    private void Start()
    {
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(HunterState newState)
    {
        currentState?.Exit();

        currentState = newState;

        currentState.Enter();
    }

    public void ConsumeEnergy(float amount)
    {
        currentEnergy -= amount * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
    }

    public void RestoreEnergy()
    {
        currentEnergy = maxEnergy;
    }

    public bool HasEnergy()
    {
        return currentEnergy > 0f;
    }

    public void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypointIndex];

        Vector3 steeringForce = steering.Arrive(target.position);
        steering.Move(steeringForce);

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            AdvanceWaypoint();
        }
    }

    private void AdvanceWaypoint()
    {
        if (loopWaypoints)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            return;
        }

        currentWaypointIndex += patrolDirection;

        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = waypoints.Length - 2;
            patrolDirection = -1;
        }
        else if (currentWaypointIndex < 0)
        {
            currentWaypointIndex = 1;
            patrolDirection = 1;
        }
    }

    public Boid FindClosestBoid()
    {
        Collider[] boids = Physics.OverlapSphere(
            transform.position,
            visionRange,
            boidLayer
        );

        Boid nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider hit in boids)
        {
            Boid boid = hit.GetComponent<Boid>();

            if (boid == null)
                continue;

            float dist = Vector3.Distance(
                transform.position,
                boid.transform.position
            );

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = boid;
            }
        }

        return nearest;
    }

    public bool CanSeeBoid()
    {
        return FindClosestBoid() != null;
    }

    public void Hunt(Boid target)
    {
        if (target == null)
            return;

        Vector3 steeringForce = steering.Pursuit(
            target.transform,
            target.GetVelocity()
        );

        steering.Move(steeringForce);
    }

    public Vector3 GetVelocity()
    {
        return steering.Velocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = visionColor;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = waypointColor;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null)
                continue;

            Gizmos.DrawSphere(waypoints[i].position, 0.4f);

            if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(
                    waypoints[i].position,
                    waypoints[i + 1].position
                );
            }
        }
    }
}
