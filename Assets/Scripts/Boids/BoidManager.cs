using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance;

    public List<Boid> boids = new List<Boid>();
    public List<Food> foods = new List<Food>();

    public Hunter hunter;

    void Awake()
    {
        Instance = this;
    }

    public List<Boid> GetNeighbors(Boid boid)
    {
        List<Boid> neighbors = new List<Boid>();

        foreach (var b in boids)
        {
            if (b != boid && Vector3.Distance(b.transform.position, boid.transform.position) < 5f)
                neighbors.Add(b);
        }

        return neighbors;
    }

    public Food GetClosestFood(Vector3 pos, float range)
    {
        Food closest = null;
        float minDist = Mathf.Infinity;

        foreach (var f in foods)
        {
            float dist = Vector3.Distance(pos, f.transform.position);
            if (dist < range && dist < minDist)
            {
                minDist = dist;
                closest = f;
            }
        }

        return closest;
    }
}
