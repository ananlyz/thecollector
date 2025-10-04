using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public int maxJumpCount = 2;

    private Rigidbody2D rb;
    private int jumpCount = 0;
    public bool isGrounded;       // 是否与地面接触
    private float moveInput;
    private float moveInputY;

    [Header("地面检测")]
    public Transform groundCheck;  // 地面检测点
    public float checkRadius = 0.2f; // 检测半径
    public LayerMask groundLayer;    // 地面层Layer=Ground

    private Animator animator;
    private bool isHurt = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isHurt)
        {
            /* hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0)
            {
                isHurt = false;
                animator.SetBool("IsHurt", false);
            } */
            return;
        }
        moveInput = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
    }

    private void DoJump()
    {
        if (jumpCount < maxJumpCount)
        {
            ++jumpCount;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (!isGrounded) animator.SetBool("HasDoubleJump", true);
        }
    }

    void FixedUpdate()
    {
        if (isHurt) return;
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
        animator.SetBool("IsRun", Mathf.Abs(moveInput) > 0f);
        if (moveInputY < 0)
        {
            animator.SetBool("IsCrouch", true);
        }
        else
        {
            animator.SetBool("IsCrouch", false);
        }

        // 贴墙处理
        bool onWallRight = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, groundLayer);
        bool onWallLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, groundLayer);
        if ((onWallLeft || onWallRight) && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded)
        {
            jumpCount = 0;
            animator.SetBool("IsJump", false);
            animator.SetBool("IsDoubleJump", false);
            animator.SetBool("HasDoubleJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < maxJumpCount)
            {
                PerformJump();
            }
        }

    }

    private void PerformJump()
    {
        if (isGrounded) animator.SetBool("IsJump", true);
        else if (!isGrounded) animator.SetBool("IsDoubleJump", true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 0.6f);
    }

    public void OnHurtEnd() // huidiao
    {
        isHurt = false;
        animator.SetBool("IsHurt", false);
    }

    public void TaskHurt(Vector2 hitDirection, float knockbackForce)
    {
        if (isHurt) return;
        isHurt = true;
        animator.SetBool("IsHurt", true);
        rb.velocity = Vector2.zero;
        // jitui rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);
    }
}