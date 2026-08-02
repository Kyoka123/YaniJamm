using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject poofEffectPrefab;
    public int maxHealth = 90;
    public int currentHealth;
    public bool isKnockedBack = false;
    public float knockbackDuration = 0.25f;
    private IEnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = GetComponent<IEnemyMovement>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
            return;
        }

        if (rb != null && knockback != Vector2.zero)
        {
            StopAllCoroutines();
            StartCoroutine(ApplyKnockback(knockback));
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockback)
    {
        if (knockback != Vector2.zero)
        {
            isKnockedBack = true;

            if (enemyMovement != null)
            {
                enemyMovement.OnKnockbackStart();
            }

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(knockback, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(knockbackDuration);

            isKnockedBack = false;
        }
    }

    private IEnumerator Die()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (poofEffectPrefab != null)
        {
            GameObject poofEffect = Instantiate(poofEffectPrefab, transform.position, Quaternion.identity);
            Animator poofAnimator = poofEffect.GetComponent<Animator>();
            if (poofAnimator != null)
            {
                poofAnimator.Play("Poof", 0, 0f);
            }
            Destroy(poofEffect, 1f);
        }

        yield return new WaitForSeconds(0.6f);

        Destroy(gameObject);
    }
}