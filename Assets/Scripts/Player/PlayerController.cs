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

    [Space]
    [Header("Animation")]
    [SerializeField] private Animator playerAnim;

    //[Space]
    //[Header("Sound Effect")]
    //[SerializeField] private ParticleSystem runOnGround;
    //[SerializeField] private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        Move();
        //playerAnim.SetFloat("Speed", Mathf.Abs(horizontal));
        //playerAnim.SetBool("IsGrounded", isGrounded);
        Flip(horizontal);
        if (Input.GetKeyDown(KeyCode.W) && CheckGround())
        {
            Jump();
            //playerAnim.SetTrigger("Jump");
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * jumpForce * Mathf.Sign(rb.gravityScale);
    }

    private bool CheckGround()
    {
        float extraHeight = 0.5f; 
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
}
