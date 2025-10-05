using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;
    public float jumpForce = 5f;
    public int maxJumpCount = 2;

    private Rigidbody2D rb;
    private int jumpCount = 0;
    public bool isGrounded;       // 是否与地面接触
    private float moveInputX;
    private float moveInputY;

    [Header("地面检测")]
    public Transform groundCheck;  // 地面检测点
    public float checkRadius = 0.2f; // 检测半径
    public LayerMask groundLayer;    // 地面层Layer=Ground

    private Animator animator;
    private bool isHurt = false;
    private bool isClimb = false;
    private bool isOnLadder = false;
    private float gravityScaleAtStart;
    private bool isWaiting = false;

    public void SetPlayerState(bool waiting)
    {
        isWaiting = waiting;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravityScaleAtStart = rb.gravityScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOnLadder = true;
                animator.SetBool("IsOnLadder", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isOnLadder = false;
            isClimb = false;
            animator.SetBool("IsClimb", false);
            animator.SetBool("IsOnLadder", false);
            rb.gravityScale = gravityScaleAtStart;
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        if (isHurt || isWaiting) // 利用动画事件回调
        {
            /* hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0)
            {
                isHurt = false;
                animator.SetBool("IsHurt", false);
            } */
            return;
        }
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
        if (isOnLadder)
        {
            isClimb = moveInputY != 0;
            animator.SetBool("IsClimb", isClimb);
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, moveInputY * climbSpeed);
        }
        if (!isOnLadder)
        {
            isClimb = false;
            rb.gravityScale = gravityScaleAtStart;
            animator.SetBool("IsClimb", false);
        }
        if (isGrounded)
        {
            jumpCount = 0;
            animator.SetBool("IsJump", false);
            animator.SetBool("IsDoubleJump", false);
            animator.SetBool("HasDoubleJump", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            PerformJump();
            //test
        }
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
        

        if (isHurt || isWaiting) return;
        
        if (moveInputX != 0 && !isOnLadder) // 反转
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInputX), 1, 1);
        }
        animator.SetBool("IsRun", Mathf.Abs(moveInputX) > 0f);
        if (moveInputY < 0 && !isOnLadder && isGrounded) // 下蹲
        {
            animator.SetBool("IsCrouch", true);
        }
        else
        {
            animator.SetBool("IsCrouch", false);
        }

        //TODO: 贴墙处理 
        /* bool onWallRight = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, groundLayer);
        bool onWallLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, groundLayer);
        if ((onWallLeft || onWallRight) && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInputX * moveSpeed, rb.velocity.y);
        } */

        if (isGrounded && !isOnLadder)
        {
            rb.velocity = new Vector2(moveInputX * moveSpeed, rb.velocity.y);
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
        // jitui 
        rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);
    }
}