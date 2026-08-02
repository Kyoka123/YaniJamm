using UnityEngine;

public class Enemy_0 : MonoBehaviour
{
    Enemy Health;
    public Transform player;
    public float rollSpeed = 5f;
    public int HpUp = -15;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        Health = Object.FindFirstObjectByType<Enemy>();
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

        FacePlayer();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float direction = player.position.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * rollSpeed, rb.linearVelocity.y);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Boy playerHealth = collision.GetComponent<Boy>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(HpUp);
            }
            OnPlayerHit(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerHit(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnPlayerHit(GameObject playerObject)
    {
        // Player'a degdiginde calisacak kodunu buraya yazacaksin
        Health.currentHealth = 0;
       
    }
}
