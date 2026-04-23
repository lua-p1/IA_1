using System.Collections.Generic;
using UnityEngine;

public class BoidPerception
{
    Boid boid;

    public BoidPerception(Boid boid)
    {
        this.boid = boid;
    }

    public Food GetFood()
    {
        return BoidManager.Instance.GetClosestFood(boid.transform.position, boid.foodRange);
    }

    public Hunter GetHunter()
    {
        var h = BoidManager.Instance.hunter;

        if (h != null && Vector3.Distance(boid.transform.position, h.transform.position) < boid.hunterRange)
            return h;

        return null;
    }

    public List<Boid> GetNeighbors()
    {
        return BoidManager.Instance.GetNeighbors(boid);
    }
}
