using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnEnable()
    {
        BoidManager.Instance.foods.Add(this);
    }

    private void OnDestroy()
    {
        if (BoidManager.Instance != null)
            BoidManager.Instance.foods.Remove(this);
    }
}
