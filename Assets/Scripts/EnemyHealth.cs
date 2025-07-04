using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public EnemySpawner spawner;

    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} HP tersisa: {currentHealth}");

        StartCoroutine(HitEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator HitEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} telah mati.");

        if (spawner != null)
        {
            spawner.RespawnEnemy(transform.position, 3f);
        }

        Destroy(gameObject);
    }
}
