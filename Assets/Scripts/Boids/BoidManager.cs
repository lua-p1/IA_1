using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance;

    public List<Boid> boids = new List<Boid>();
    public List<Food> foods = new List<Food>();

    public Hunter hunter;

    [Header("Spawn")]
    public GameObject boidPrefab;
    public int amount = 20;
    public float spawnRange = 10f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnBoids();
    }

    void SpawnBoids()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                1,
                Random.Range(-spawnRange, spawnRange)
            );

            Instantiate(boidPrefab, pos, Quaternion.identity);
        }
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
