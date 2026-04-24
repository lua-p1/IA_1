using UnityEngine;

public class Food : MonoBehaviour
{
    [HideInInspector] public BoidManager manager;

    void OnEnable()
    {
        manager?.foods.Add(this);
    }

    void OnDisable()
    {
        manager?.foods.Remove(this);
    }

    void OnTriggerEnter(Collider other)
    {
        Boid boid = other.GetComponent<Boid>();

        if (boid != null)
        {
            manager.OnFoodConsumed(this);
        }
    }
}