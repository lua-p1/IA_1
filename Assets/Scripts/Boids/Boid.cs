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

    void Start()
    {
        Perception = new BoidPerception(this);
        decisionTree = new BoidDecisionTree();

        velocity = Random.insideUnitSphere * maxSpeed;
    }

    void Update()
    {
        var behavior = decisionTree.Decide(this);
        Move(behavior.Calculate(this));

        if (velocity != Vector3.zero)
            transform.forward = velocity;
    }

    void Move(Vector3 desired)
    {
        Vector3 steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        velocity += steering * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * Time.deltaTime;
    }
}
