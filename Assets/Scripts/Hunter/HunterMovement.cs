using UnityEngine;
public class HunterMovement
{
    Hunter hunter;

    public HunterMovement(Hunter h)
    {
        hunter = h;
    }

    public void Move(Vector3 dir)
    {
        Vector3 velocity = dir.normalized * hunter.maxSpeed;

        hunter.transform.position += velocity * Time.deltaTime;
        hunter.transform.forward = velocity;
    }
}
