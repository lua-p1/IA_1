using UnityEngine;

public class Food : MonoBehaviour
{
    void OnEnable()
    {
        if (BoidManager.Instance != null)
            BoidManager.Instance.foods.Add(this);
    }
    private void OnDestroy()
    {
        if (BoidManager.Instance != null)
            BoidManager.Instance.foods.Remove(this);
    }
}
