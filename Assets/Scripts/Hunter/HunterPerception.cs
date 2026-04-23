using UnityEngine;

public class HunterPerception
{
    Hunter hunter;

    public HunterPerception(Hunter hunter)
    {
        this.hunter = hunter;
    }

    public Boid GetTarget()
    {
        Boid closest = null;
        float minDist = Mathf.Infinity;

        foreach (var b in BoidManager.Instance.boids)
        {
            float dist = Vector3.Distance(hunter.transform.position, b.transform.position);

            if (dist < hunter.visionRange && dist < minDist)
            {
                minDist = dist;
                closest = b;
            }
        }

        return closest;
    }
}
