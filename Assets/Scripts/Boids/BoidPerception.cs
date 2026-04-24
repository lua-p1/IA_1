using System.Collections.Generic;
using UnityEngine;
public class BoidPerception
{
    Boid boid;
    BoidManager manager;

    public BoidPerception(Boid boid)
    {
        this.boid = boid;
        this.manager = boid.manager;
    }

    public Food GetFood()
    {
        return manager.GetClosestFood(boid.transform.position, boid.foodRange);
    }

    public Hunter GetHunter()
    {
        var h = manager.hunter;

        if (h != null && Vector3.Distance(boid.transform.position, h.transform.position) < boid.hunterRange)
            return h;

        return null;
    }

    public List<Boid> GetNeighbors()
    {
        return manager.GetNeighbors(boid);
    }
}
