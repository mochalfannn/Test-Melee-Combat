using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public void RespawnEnemy(Vector3 position, float delay)
    {
        StartCoroutine(RespawnCoroutine(position, delay));
    }

    private IEnumerator RespawnCoroutine(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
       
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.spawner = this;
        }

        Debug.Log("Enemy respawned.");
    }
}
