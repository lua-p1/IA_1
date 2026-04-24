using UnityEngine;

public class Boid : MonoBehaviour
{
    public float maxSpeed = 5;
    public float maxForce = 10;
    public float slowRadius = 3;
    public float foodRange = 10;
    public float hunterRange = 12;

    Vector3 velocity;

    IDecisionNode decisionTree;
    public BoidPerception Perception { get; private set; }

    public Vector3 Velocity => velocity;

    [HideInInspector] public BoidManager manager;

    void OnEnable()
    {
        manager?.boids.Add(this);
    }

    void OnDisable()
    {
        manager?.boids.Remove(this);
    }

    void Start() { }
    public void Initialize()
    {
        Perception = new BoidPerception(this);
        decisionTree = new BoidDecisionTree();

        velocity = Random.insideUnitSphere * maxSpeed;
        velocity.y = 0;
        Debug.Log("Boid inicializado");
    }

    void Update()
    {
        if (decisionTree == null) return;

        var behavior = decisionTree.Decide(this);
        Move(behavior.Calculate(this));

        if (velocity != Vector3.zero)
        {
            Vector3 forward = velocity;
            forward.y = 0;
            transform.forward = forward;
        }
    }

    void Move(Vector3 desired)
    {
        desired.y = 0;

        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        velocity += steering * Time.deltaTime;
        velocity.y = 0;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        Vector3 nextPos = transform.position + velocity * Time.deltaTime;

        if (!Physics.Raycast(transform.position, velocity.normalized, 1f))
        {
            transform.position = nextPos;
        }

        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
    }
}