using UnityEngine;

public class Enemy_0 : MonoBehaviour, IEnemyMovement
{
    public Transform player;
    public float rollSpeed = 5f;
    private int HpUp = -20;

    private Rigidbody2D rb;
    private Animator animator;
    private Enemy enemyScript;

    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("Roll");
        }
    }

    void Update()
    {
        if (player == null) return;

        if (enemyScript != null && enemyScript.isKnockedBack) return;

        FacePlayer();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (enemyScript != null && enemyScript.isKnockedBack) return;

        float direction = player.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * rollSpeed, rb.linearVelocity.y);
    }

    public void OnKnockbackStart()
    {
    }

    private void FacePlayer()
    {
        float direction = player.position.x - transform.position.x;

        if (direction > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Boy playerHealth = collision.gameObject.GetComponent<Boy>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(HpUp);
            }

            OnPlayerHit();
        }
    }

    private void OnPlayerHit()
    {
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(enemyScript.currentHealth, Vector2.zero);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}