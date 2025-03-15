using pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizontal;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private float jumpTime = 1.185f;
    private float countTime = 0;

    [Space]
    [Header("Animation")]
    [SerializeField] private Animator playerAnim;

    [Space]
    [Header("Skill")]
    [SerializeField] private GameObject checkPoint;
    private Vector3 checkPointPos;
    private bool isSettingPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        playerAnim.SetFloat("Speed", Mathf.Abs(horizontal));
        playerAnim.SetBool("IsGround", CheckGround());
        Move();
        Flip(horizontal);
        if (Input.GetKeyDown(KeyCode.W) && CheckGround())
        {
            Jump();
        }
        else if (countTime <= 0)
        {
            playerAnim.SetBool("IsJumping", false);
        }

        if (countTime > 0)
        {
            countTime -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SetCheckPoint();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            BackToCheckPoint();
        }
    }

    private void OnLanding()
    {
        if (CheckGround())
        {
            playerAnim.SetBool("IsJumping", false);
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        countTime = jumpTime;
        rb.velocity = Vector2.up * jumpForce * Mathf.Sign(rb.gravityScale);
        playerAnim.SetBool("IsJumping", true);
    }

    private bool CheckGround()
    {
        float extraHeight = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeight, groundLayer);
        return hit.collider != null;
    }

    private void Flip(float direction)
    {
        if ((direction > 0 && spriteRenderer.flipX) || (direction < 0 && !spriteRenderer.flipX))
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            Rigidbody2D pushObj = collision.gameObject.GetComponent<Rigidbody2D>();
            if (pushObj != null)
            {
                pushObj.velocity = new Vector2(rb.velocity.x / 2f, pushObj.velocity.y * 0.5f);
            }
        }
    }

    private void SetCheckPoint()
    {
        checkPointPos = transform.position;
        isSettingPoint = true;
        checkPoint.gameObject.transform.position = checkPointPos;
        checkPoint.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
    }

    private void BackToCheckPoint()
    {
        this.transform.position = new Vector3(checkPointPos.x, checkPointPos.y + 0.5f, checkPointPos.z);
        isSettingPoint = false;
        checkPoint.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0);
    }
}
