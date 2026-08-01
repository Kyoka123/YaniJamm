using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Enemy_2 : MonoBehaviour
{
    public Transform player;
    public float jumpForceY = 2f;
    public float moveSpeedX = 1.5f;
    public float jumpInterval = 1.5f;

    [Header("Zemin Ayarlari")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.05f;
    public LayerMask groundLayer;

    public SpriteRenderer childSpriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    private Enemy enemyScript;
    private bool isGrounded;
    private float nextJumpTime;
    private bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<Enemy>();
    }

    void Update()
    {
        if (player == null) return;

        if (isGrounded && !isJumping)
        {
            animator.Play("Idle");
        }

        if (isGrounded && Time.time >= nextJumpTime)
        {
            StartCoroutine(JumpTowardsPlayer());
            nextJumpTime = Time.time + jumpInterval;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !isJumping)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    private IEnumerator JumpTowardsPlayer()
    {
        isJumping = true;

        float direction = player.position.x > transform.position.x ? 1f : -1f;

        animator.Play("Jump");

        yield return new WaitForSeconds(0.25f);

        rb.linearVelocity = new Vector2(direction * moveSpeedX, jumpForceY);

        animator.Play("Jump");

        yield return new WaitForSeconds(0.6f);
        isJumping = false;
    }

    private void TriggerDeath()
    {
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(enemyScript.currentHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Boy playerScript = collision.gameObject.GetComponent<Boy>();
            if (playerScript != null)
            {
                playerScript.StartCoroutine(StunPlayerRoutine(playerScript, 2f));
            }

            TriggerDeath();
        }
    }

    private IEnumerator StunPlayerRoutine(Boy playerScript, float duration)
    {
        childSpriteRenderer.enabled = true;
        playerScript.haveEraser = false;
        yield return new WaitForSeconds(duration);
        childSpriteRenderer.enabled = false;
        playerScript.haveEraser = true;
    }


}
