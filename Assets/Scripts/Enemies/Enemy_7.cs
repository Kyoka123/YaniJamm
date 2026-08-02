using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy_7 : MonoBehaviour, IEnemyMovement
{
    public Transform player;
    public float jumpForceY = 2f;
    public float moveSpeedX = 1.5f;
    public float jumpInterval = 1.5f;

    [Header("Ates Sistemi")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 6f;
    public float fireRate = 2f;
    private float nextFireTime;
    private bool isAttacking;

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

        if (distanceToPlayer <= 4f && isGrounded && !isJumping && Time.time >= nextFireTime && !isAttacking)
        {
            StartCoroutine(Shoot());
            nextFireTime = Time.time + fireRate;
        }

        if (distanceToPlayer > 4f && isGrounded && Time.time >= nextJumpTime && !isAttacking)
        {
            StartCoroutine(JumpTowardsPlayer());
            nextJumpTime = Time.time + jumpInterval;
        }
    }

    public void OnKnockbackStart()
    {
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

    private IEnumerator Shoot()
    {
        isAttacking = true;
        animator.Play("Attack");

        yield return new WaitForSeconds(0.1f);

        float facingDirection = transform.localScale.x > 0 ? 1f : -1f;
        Vector3 spawnOffset = new Vector3(0.33f * facingDirection, 0.24f, 0f);
        Vector3 spawnPosition = transform.position + spawnOffset;

        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            if (facingDirection < 0)
            {
                bullet.transform.localScale = new Vector3(-Mathf.Abs(bullet.transform.localScale.x), bullet.transform.localScale.y, bullet.transform.localScale.z);
            }

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = new Vector2(facingDirection * bulletSpeed, 0f);
            }
        }

        yield return new WaitForSeconds(0.65f);
        isAttacking = false;
        
    }

    private IEnumerator JumpTowardsPlayer()
    {
        isJumping = true;

        float direction = player.position.x > transform.position.x ? 1f : -1f;

        animator.Play("Jump");

        rb.linearVelocity = new Vector2(direction * moveSpeedX, jumpForceY);

        while (!isGrounded)
        {

        }

        animator.Play("Jump");

        yield return new WaitForSeconds(0.2f);
        isJumping = false;
    }
}
