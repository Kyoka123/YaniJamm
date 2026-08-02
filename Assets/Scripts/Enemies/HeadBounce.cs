using UnityEngine;

public class HeadBounce : MonoBehaviour
{
    public float bounceForceX = 4f;
    public float bounceForceY = 4f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D targetRb = collision.GetComponent<Rigidbody2D>();

        if (targetRb != null)
        {
            float direction = collision.transform.position.x > transform.position.x ? 1f : -1f;

            targetRb.linearVelocity = Vector2.zero;
            Vector2 pushForce = new Vector2(direction * bounceForceX, bounceForceY);
            targetRb.AddForce(pushForce, ForceMode2D.Impulse);
        }

        if (collision.CompareTag("Player"))
        {
            Boy playerController = collision.GetComponent<Boy>();
            if (playerController != null)
            {
                if (gameObject.transform.parent.name == "Enemy_0") return;
                playerController.TakeDamage(10);
            }
        }
    }
}