using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public AudioClip hurtSFX;
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;
    public float jumpForce = 5f;
    public int maxJumpCount = 2;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    public float crouchHeight = 0.5f;
    public float tSpeed = 10f;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private Vector2 targetColliderSize;
    private Vector2 targetColliderOffset;
    private int jumpCount = 0;
    public bool isGrounded;       // 是否与地面接触
    private float moveInputX;
    private float moveInputY;

    [Header("地面检测")]
    public Transform groundCheck;  // 地面检测点
    public float checkRadius = 0.2f; // 检测半径
    public LayerMask groundLayer;    // 地面层Layer=Ground

    [Header("墙面检测")]
    public Transform wallCheck;
    public float wallCheckDistance = 1f;
    public LayerMask wallLayer;

    private Animator animator;
    private bool isHurt = false;
    private bool isClimb = false;
    private bool isOnLadder = false;
    private float gravityScaleAtStart;
    private bool isWaiting = false;

    private bool isFaceWall = false; // 是否面向墙面
    private bool isOntheWall = false; // 是否吸附在墙面
    private bool isOnWallRun = false;
    public bool isWallJump = false;
    

    public void SetPlayerState(bool waiting)
    {
        isWaiting = waiting;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();

        originalColliderSize = col.bounds.size;
        originalColliderOffset = col.offset;
        targetColliderSize = originalColliderSize;
        targetColliderOffset = originalColliderOffset;
        gravityScaleAtStart = rb.gravityScale;
    }
    private void OnTriggerEnter2D(Collider2D other)
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
        if (isOntheWall)
        {
            animator.SetBool("IsStickeToWall", true);
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
            animator.SetBool("IsGrounded", isGrounded);
            if (isGrounded) // 如果从吸附墙的状态接触到地面 直接退出吸附状态
            {
                isOntheWall = false;
                animator.SetBool("IsWallToGround", true);
                animator.SetBool("IsStickeToWall", false);
                return;
            }
            moveInputY = Input.GetAxisRaw("Vertical");
            //moveInputX = Input.GetAxisRaw("Horizontal");

            jumpCount = 0;
            animator.SetBool("IsJump", false);
            animator.SetBool("IsDoubleJump", false);
            animator.SetBool("HasDoubleJump", false);
            animator.SetBool("IsWallJump", false);
            isWallJump = false;

            isOnWallRun = moveInputY != 0;
            animator.SetBool("IsOnWallRun", isOnWallRun);
            rb.gravityScale = 0.5f;
            rb.velocity = new Vector2(0, moveInputY * climbSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isOntheWall = false;
                animator.SetBool("IsStickeToWall", false);

                animator.SetBool("IsWallToGround", false);

                isWallJump = true;
                animator.SetBool("IsWallJump", true);
                Debug.Log("this wall jump");

                //rb.AddForce(Vector2.left * transform.localScale.x * jumpForce, ForceMode2D.Impulse);
                //rb.AddForce(Vector2.right * jumpForce, ForceMode2D.Impulse);
                rb.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * jumpForce, jumpForce);
                isOntheWall = false;
                //DoWallJump();
            }
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        isFaceWall = Physics2D.Raycast(wallCheck.position, Vector2.right * transform.localScale.x, wallCheckDistance, wallLayer);
        isOntheWall = (!isGrounded && isFaceWall);
        if (isOntheWall) return;
        isOnWallRun = false;
        animator.SetBool("IsOnWallRun", false);

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
            animator.SetBool("IsClimb", false);
            rb.gravityScale = gravityScaleAtStart;
        }
        if (isGrounded)
        {
            jumpCount = 0;
            animator.SetBool("IsJump", false);
            animator.SetBool("IsDoubleJump", false);
            animator.SetBool("HasDoubleJump", false);

            animator.SetBool("IsWallJump", false);
            isWallJump = false;

            animator.SetBool("IsWallToGround", false);
            animator.SetBool("IsStickeToWall", false);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformJump();
        }
        if (moveInputY < 0 && !isOnLadder && isGrounded) // 下蹲
        {
            targetColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y * crouchHeight);
            targetColliderOffset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - (originalColliderSize.y - targetColliderSize.y)/2f);
            animator.SetBool("IsCrouch", true);
        }
        else
        {
            targetColliderSize = originalColliderSize;
            targetColliderOffset = originalColliderOffset;
            animator.SetBool("IsCrouch", false);
        }
        
        // 平滑过渡
        col.size = Vector2.Lerp(col.bounds.size, targetColliderSize, Time.deltaTime * tSpeed);
        col.offset = Vector2.Lerp(col.offset, targetColliderOffset, Time.deltaTime * tSpeed);
    }
    
    private void PerformJump()
    {
        if (isGrounded) animator.SetBool("IsJump", true);
        else if (!isGrounded && !isOntheWall) animator.SetBool("IsDoubleJump", true);
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

    private void DoWallJump()
    {
        if (jumpCount < maxJumpCount)
        {
            Debug.Log("wall jump");
            ++jumpCount;
            //rb.AddForce(Vector2.left * transform.localScale.x * jumpForce, ForceMode2D.Impulse);
            rb.velocity = new Vector2(-1 * transform.localScale.x * jumpForce, jumpForce);
            /* isWallJump = false;
            animator.SetBool("IsWallJump", false);  */
        }  
    }

    void FixedUpdate()
    {
        
        if (isHurt || isWaiting || isOntheWall)
        {
            animator.SetBool("IsCrouchWalk", false);
            animator.SetBool("IsRun", false);
            return;
        }
        
        if (moveInputX != 0 && !isOnLadder) // 反转
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInputX), 1, 1);
        }

        rb.velocity = new Vector2(moveInputX * moveSpeed, rb.velocity.y);

        if (!isGrounded)
        {
            animator.SetBool("IsCrouchWalk", false);
            animator.SetBool("IsRun", false);
            return;
        }

        if (moveInputY < 0) animator.SetBool("IsCrouchWalk", Mathf.Abs(moveInputX) > 0f);
        else animator.SetBool("IsRun", Mathf.Abs(moveInputX) > 0f);
        

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

    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 0.6f);
        Gizmos.DrawLine(wallCheck.transform.position, wallCheck.transform.position + Vector3.right * wallCheckDistance);
    }

    public void OnHurtEnd() // huidiao
    {
        isHurt = false;
        animator.SetBool("IsHurt", false);
    }

    public void TaskHurt(Vector2 hitDirection, float knockbackForce)
    {
        if (isHurt) return;
        AudioSource.PlayClipAtPoint(hurtSFX, transform.position, 2.0f);
        isHurt = true;
        animator.SetBool("IsHurt", true);
        rb.velocity = Vector2.zero;
        // jitui 
        rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);
    }
}