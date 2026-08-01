using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject poofEffectPrefab;
    public int maxHealth = 90;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage. Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        GameObject poofEffect = Instantiate(poofEffectPrefab, transform.position, Quaternion.identity);
        Animator poofAnimator = poofEffect.GetComponent<Animator>();
        if (poofAnimator != null)
        {
            poofAnimator.Play("Poof");
        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.6f);

        Destroy(poofEffect);
        Destroy(gameObject);
    }
}
