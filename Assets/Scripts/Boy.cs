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
    public int attackDamage = 30;

    [Header("Geri Püskürtme (Knockback)")]
    public float knockbackForceX = 3f;
    public float knockbackForceY = 2f;

    public float attackRate = 1.5f;
    private float nextAttackTime;

    private bool isJab = true;
    private bool isAttacking = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping;
    public bool isStunned;
    public bool haveEraser = true;
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 2f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.05f;
    public LayerMask groundLayer;

    private float currentInput;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        currentInput = (Keyboard.current.rightArrowKey.isPressed ? 1 : 0) - (Keyboard.current.leftArrowKey.isPressed ? 1 : 0);

        if (Time.time >= nextAttackTime && Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame && !isAttacking && !isStunned && haveEraser)
        {
            if (!isGrounded)
            {
                isJab = false;
            }
            StartCoroutine(PerformAttack());
            nextAttackTime = Time.time + (1f / attackRate);
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame && isGrounded && !isStunned)
        {
            isJumping = true;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            haveEraser = !haveEraser;
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            isStunned = !isStunned;
        }

        Animation(currentInput);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Move();
    }

    private void Move()
    {
        if ((isAttacking && isGrounded) || isStunned)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(currentInput * moveSpeed, rb.linearVelocity.y);

        if (currentInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (currentInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = false;
        }
    }

    private void Animation(float input)
    {
        if (isStunned)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
            {
                animator.Play("Stun", 0, 0f);
            }
            return;
        }

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
            ApplyDamage(attackDamage);
            yield return new WaitForSeconds(0.3f);
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

        float facingDirection = transform.localScale.x > 0 ? 1f : -1f;

        Vector2 knockbackVector = new Vector2(facingDirection * knockbackForceX, knockbackForceY);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, knockbackVector);
            }
        }
    }
}