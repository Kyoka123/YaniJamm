using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_9 : MonoBehaviour
{
    public Transform player;
    public float jumpForceY = 2f;
    public float moveSpeedX = 1.5f;
    public float jumpInterval = 1.5f;

    [Header("Saldiri Ayarlari")]
    public Collider2D attackCollider;
    public LayerMask targetLayers;
    public float attackCooldown = 1f;

    private float nextAttackTime;
    private bool isAttacking;

    [Header("Zemin Ayarlari")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.05f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float nextJumpTime;
    private bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();

        if (isGrounded && !isJumping && !isAttacking)
        {
            animator.Play("Idle");
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= 0.8f && isGrounded && !isJumping && Time.time >= nextAttackTime && !isAttacking)
        {
            StartCoroutine(PerformMeleeAttack());
            nextAttackTime = Time.time + attackCooldown;
        }

        if (distanceToPlayer > 0.8f && isGrounded && Time.time >= nextJumpTime && !isAttacking)
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

    private IEnumerator PerformMeleeAttack()
    {
        isAttacking = true;
        animator.Play("Attack");

        yield return new WaitForSeconds(0.6f);

        ApplyMeleeDamage();

        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    private void ApplyMeleeDamage()
    {
        if (attackCollider == null) return;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayers);
        filter.useLayerMask = true;
        filter.useTriggers = true;

        List<Collider2D> hitTargets = new List<Collider2D>();
        attackCollider.Overlap(filter, hitTargets);

        foreach (Collider2D hit in hitTargets)
        {
            // Custom collider icinde kalan hedeflere uygulanacak hasar kodunu buraya yazacaksin

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
}
