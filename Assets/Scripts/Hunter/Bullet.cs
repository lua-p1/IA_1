using UnityEngine;
public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float lifeTime = 5f;
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Boid"))
        {
            Debug.Log("Impacto confirmado en Boid");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
