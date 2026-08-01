using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class Boy : MonoBehaviour
{
    [Header("Saldiri Alani")]
    public Collider2D attackCollider;
    public LayerMask enemyLayers;
    public int attackDamage = 20;

    public float attackRate = 1f;
    private float nextAttackTime;

    private bool isJab = true;
    private bool isAttacking = false;

    private Vector2 storedVelocity;
    private float originalGravityScale;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    private bool haveEraser = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 2f;
    public float jumpForce = 4f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.05f;
    public LayerMask groundLayer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (Time.time >= nextAttackTime && Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame && !isAttacking)
        {
            StartCoroutine(PerformAttack());
            nextAttackTime = Time.time + (1f / attackRate);
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && isGrounded)
        {
            isJumping = true;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            haveEraser = !haveEraser;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Move();
    }

    private void Move()
    {
        if (isAttacking) return;

        float input = (Keyboard.current.rightArrowKey.isPressed ? 1 : 0) - (Keyboard.current.leftArrowKey.isPressed ? 1 : 0);
        rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);

        if (input > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (input < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = false;
        }

        Animation(input);
    }

    private void Animation(float input)
    {
        if (isAttacking) return;

        if (!haveEraser)
        {
            if (isGrounded)
            {
                animator.Play(input == 0 ? "IdleNormal" : "RunningNormal");
            }
            else
            {
                animator.Play(rb.linearVelocity.y > 0 ? "JumpNormal" : "FallNormal");
            }
        }
        else
        {
            if (isGrounded)
            {
                animator.Play(input == 0 ? "IdleEraser" : "RunningEraser");
            }
            else
            {
                animator.Play(rb.linearVelocity.y > 0 ? "JumpEraser" : "FallEraser");
            }
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        storedVelocity = new Vector2(rb.linearVelocity.x, 0f);

        if (isGrounded)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        if (isJab)
        {
            animator.Play("EraserJab");
            yield return new WaitForSeconds(0.3f);
            ApplyDamage(attackDamage);
            yield return new WaitForSeconds(0.36f);
        }
        else
        {
            animator.Play("EraserSlash");
            yield return new WaitForSeconds(0.36f);
            ApplyDamage(attackDamage + 5);
            yield return new WaitForSeconds(0.3f);
        }

        rb.gravityScale = originalGravityScale;

        if (isGrounded)
        {
            rb.linearVelocity = storedVelocity;
        }

        isJab = !isJab;
        isAttacking = false;
    }

    private void ApplyDamage(int damage)
    {
        if (attackCollider == null) return;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(enemyLayers);
        filter.useLayerMask = true;
        filter.useTriggers = true;

        List<Collider2D> hitEnemies = new List<Collider2D>();
        attackCollider.Overlap(filter, hitEnemies);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}