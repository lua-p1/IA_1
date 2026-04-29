using System.Collections.Generic;
using UnityEngine;

public class FoodPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private FoodFactory factory;
    [SerializeField] private int poolSize = 20;

    [Header("Spawn Area")]
    [SerializeField] private Vector3 areaCenter = Vector3.zero;
    [SerializeField] private Vector3 areaSize = new Vector3(50f, 0f, 50f);

    private List<Food> foodPool = new List<Food>();

    private void Start()
    {
        GeneratePool();
    }

    private void GeneratePool()
    {
        if (factory == null)
        {
            Debug.LogError("FoodPool: Falta asignar FoodFactory.");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            Vector3 randomPos = GetRandomPosition();

            Food food = factory.CreateFood(randomPos, transform);

            if (food != null)
            {
                food.Initialize(this);
                foodPool.Add(food);
            }
        }
    }

    public Food GetInactiveFood()
    {
        foreach (Food food in foodPool)
        {
            if (!food.gameObject.activeInHierarchy)
                return food;
        }

        return null;
    }

    public Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(
            areaCenter.x - areaSize.x / 2f,
            areaCenter.x + areaSize.x / 2f
        );

        float randomZ = Random.Range(
            areaCenter.z - areaSize.z / 2f,
            areaCenter.z + areaSize.z / 2f
        );

        return new Vector3(randomX, areaCenter.y, randomZ);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            areaCenter,
            areaSize
        );
    }
}