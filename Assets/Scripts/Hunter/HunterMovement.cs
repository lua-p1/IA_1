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
        dir.y = 0;

        Vector3 velocity = dir.normalized * hunter.maxSpeed;

        hunter.transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero)
        {
            Vector3 forward = velocity;
            forward.y = 0;

            hunter.transform.forward = forward;
        }
    }
}
