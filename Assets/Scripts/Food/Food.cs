using System.Collections;
using UnityEngine;
public class Food : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private float respawnTime = 5f;

    private FoodPool pool;
    public void Initialize(FoodPool foodPool)
    {
        pool = foodPool;
    }
    public void Consume()
    {
        if (!gameObject.activeInHierarchy)
            return;
        Debug.Log("Comida consumida");
        gameObject.SetActive(false);
        if (pool != null)
            pool.StartCoroutine(RespawnCoroutine());
    }
    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);

        if (pool != null)
        {
            transform.position = pool.GetRandomPosition();
            gameObject.SetActive(true);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}