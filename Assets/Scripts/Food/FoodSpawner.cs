using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public float spawnRadius = 20;
    public float spawnInterval 3;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnFood), 1, spawnInterval);
    }

    void SpawnFood()
    {
        Vector3 pos = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        
        Instantiate(foodPrefab, pos, Quaternion.identity);
    }
}
