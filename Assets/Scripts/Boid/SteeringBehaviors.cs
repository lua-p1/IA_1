using UnityEngine;
public class SteeringBehaviors : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxForce = 0.01f;
    [SerializeField] private float arriveRadius = 3f;

    public Vector3 Velocity { get; private set; }

    public void Move(Vector3 steering)
    {
        steering = Vector3.ClampMagnitude(steering, maxForce);
        Velocity = Vector3.ClampMagnitude(Velocity + steering * Time.deltaTime,maxSpeed);
        Vector3 newPosition = transform.position + Velocity * Time.deltaTime;
        newPosition.y = 1f;
        transform.position = newPosition;
        Vector3 flatVelocity = new Vector3(Velocity.x,0f,Velocity.z);
        if (flatVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatVelocity);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 8f);
        }
    }
    public Vector3 Seek(Vector3 target)
    {
        Vector3 desired = (target - transform.position).normalized * maxSpeed;
        return desired - Velocity;
    }
    public Vector3 Arrive(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        float distance = desired.magnitude;
        float speed = maxSpeed;
        if (distance < arriveRadius)
            speed = maxSpeed * (distance / arriveRadius);
        desired = desired.normalized * speed;
        return desired - Velocity;
    }
    public Vector3 Evade(Transform target, Vector3 targetVelocity)
    {
        float prediction = Vector3.Distance(transform.position, target.position) / maxSpeed;
        Vector3 futurePos = target.position + targetVelocity * prediction;
        Vector3 desired = (transform.position - futurePos).normalized * maxSpeed;
        return desired - Velocity;
    }
    public Vector3 Pursuit(Transform target, Vector3 targetVelocity)
    {
        float prediction = Vector3.Distance(transform.position, target.position) / maxSpeed;
        Vector3 futurePos = target.position + targetVelocity * prediction;
        return Seek(futurePos);
    }
    public Vector3 Wander(float radius, float distance)
    {
        Vector3 circleCenter = Velocity.normalized * distance;
        Vector3 randomPoint = Random.insideUnitSphere * radius;
        randomPoint.y = 0;
        return circleCenter + randomPoint;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public float GetMaxForce()
    {
        return maxForce;
    }
}