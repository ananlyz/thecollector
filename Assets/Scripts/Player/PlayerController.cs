using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public int maxJumpCount = 2;

    private Rigidbody2D rb;
    private int jumpCount = 1;
    private bool isGrounded;       // 是否与地面接触
    private float moveInput;

    [Header("地面检测")]
    public Transform groundCheck;  // 地面检测点
    public float checkRadius = 0.2f; // 检测半径
    public LayerMask groundLayer;    // 地面层Layer=Ground

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGrounded)
        {
            jumpCount = 1; // 落地后重置跳跃次数
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || jumpCount < maxJumpCount)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
            }
        }
    }

    void FixedUpdate()
    {
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        // 贴墙处理
        bool onWallRight = Physics2D.Raycast(transform.position, Vector2.right, 0.6f, groundLayer);
        bool onWallLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, groundLayer);
        if ((onWallLeft || onWallRight) && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 0.6f);
    }
}