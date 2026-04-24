using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance;

    public List<Boid> boids = new List<Boid>();
    public List<Food> foods = new List<Food>();

    public Hunter hunter;

    [Header("Boids")]
    public GameObject boidPrefab;
    public int amount = 20;
    public float spawnRange = 10f;

    [Header("Food Pool")]
    public GameObject foodPrefab;
    public int initialFood = 20;
    public float spawnRadius = 20f;
    public float respawnDelay = 5f;

    List<Food> foodPool = new List<Food>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitFoodPool();
        SpawnInitialFood();
        SpawnBoids();
    }

    void SpawnBoids()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-spawnRange, spawnRange),1, Random.Range(-spawnRange, spawnRange));

            Boid b = Instantiate(boidPrefab, pos, Quaternion.identity).GetComponent<Boid>();

            b.manager = this;
            b.Initialize();
        }
    }

    void InitFoodPool()
    {
        for (int i = 0; i < initialFood; i++)
        {
            Food f = Instantiate(foodPrefab).GetComponent<Food>();
            f.manager = this;
            f.gameObject.SetActive(false);
            foodPool.Add(f);
        }
    }

    void SpawnInitialFood()
    {
        for (int i = 0; i < initialFood; i++)
            SpawnFoodFromPool();
    }

    public void SpawnFoodFromPool()
    {
        Food f = GetAvailableFood();
        f.transform.position = GetRandomPosition();
        f.gameObject.SetActive(true);
    }

    Food GetAvailableFood()
    {
        foreach (var f in foodPool)
        {
            if (!f.gameObject.activeInHierarchy)
                return f;
        }

        Food newFood = Instantiate(foodPrefab).GetComponent<Food>();
        newFood.manager = this;
        newFood.gameObject.SetActive(false);
        foodPool.Add(newFood);
        return newFood;
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-spawnRadius, spawnRadius),1f,Random.Range(-spawnRadius, spawnRadius));
    }

    public void OnFoodConsumed(Food food)
    {
        StartCoroutine(RespawnFood(food));
    }

    IEnumerator RespawnFood(Food food)
    {
        food.gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnDelay);

        food.transform.position = GetRandomPosition();
        food.gameObject.SetActive(true);
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
            if (!f.gameObject.activeInHierarchy) continue;

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
