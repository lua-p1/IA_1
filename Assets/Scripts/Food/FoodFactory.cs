using UnityEngine;

public class FoodFactory : MonoBehaviour
{
    [Header("Factory")]
    [SerializeField] private Food foodPrefab;

    public Food CreateFood(Vector3 position, Transform parent = null)
    {
        if (foodPrefab == null)
        {
            Debug.LogError("FoodFactory: Falta asignar Food Prefab.");
            return null;
        }

        Food newFood = Instantiate(
            foodPrefab,
            position,
            Quaternion.identity,
            parent
        );

        return newFood;
    }
}