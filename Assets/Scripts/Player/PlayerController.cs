using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;       // 移动速度
    public float jumpForce = 10f;      // 跳跃力度

    [Header("地面检测")]
    public Transform groundCheck;      // 地面检测点
    public float checkRadius = 0.2f;   // 检测半径
    public LayerMask groundLayer;      // 地面层

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private int jumpCount = 0;        // 跳跃计数
    public int maxJump = 2;           // 最大跳跃次数

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal"); // -1 左，1 右
        //TODO: 多层检测——检测是否在地面
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpCount = 0; // 在地面重置跳跃次数
        }

        // 多次跳
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCount < maxJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
            }
        }
    }

    void FixedUpdate()
    {
        if(moveInput != 0)
        {
            // 翻转角色方向
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}