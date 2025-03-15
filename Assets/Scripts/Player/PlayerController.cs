using System;
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
    [SerializeField] private LayerMask mapLayer;

    private bool canMove = true;

    private Rigidbody2D rb;
    private float horizontal;
    private Collider2D boxCollider2D;
    
    private SpriteRenderer spriteRenderer;
    private float jumpTime = 1.185f;
    private float countTime = 0;
    private bool canBackCheckPoint;

    [Space]
    [Header("Animation")]
    [SerializeField] private Animator playerAnim;

    [Space]
    [Header("Skill")]
    private Vector3 checkPointPos;
    private bool isSettingPoint;

    [SerializeField] private SpriteRenderer spriteCheckPointPrebfabs;
    private SpriteRenderer spriteCheckPoint;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isCheckMovePlay && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetKeyDown(KeyCode.W)))
        {
            isCheckMovePlay = false;
            CheckMovePlayer(true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetCheckPoint(); 
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if(!canBackCheckPoint) return;
            BackToCheckPoint();
        }
        if(!canMove) return;
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
    }

    private void FixedUpdate()
    {
        CheckFly();
        if(!canMove) return;
        SetParentPlayer();
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

    private void SetParentPlayer()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, Vector2.down, 0f, mapLayer);
        if (hit.collider != null) 
        {
            this.transform.SetParent(hit.transform);
        }
    }
    private bool isCheckMovePlay = false;
    private void CheckFly()
    {
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, Vector2.down, 0.5f);
        if (hit2D.Length >= 4)
        {
            CheckMovePlayer(false);
            isCheckMovePlay = true;
        }
    }

    public void CheckMovePlayer(bool enable)
    {
        if(isCheckMovePlay) return;
        rb.velocity = Vector2.zero;
        canMove = enable;
        boxCollider2D.enabled = enable;
        rb.bodyType = enable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    }
    private void SetCheckPoint()
    {
        checkPointPos = transform.position;
        isSettingPoint = true;
        spriteCheckPoint =
            PoolingManager.Spawn(spriteCheckPointPrebfabs, transform.position, Quaternion.identity);
        canBackCheckPoint = true;
    }

    private void BackToCheckPoint()
    {
        if(!spriteCheckPoint.gameObject.activeSelf) return;
        this.transform.position = new Vector3(checkPointPos.x, checkPointPos.y + 0.5f, checkPointPos.z);
        isSettingPoint = false;
        PoolingManager.Despawn(spriteCheckPoint.gameObject);
        canBackCheckPoint = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && canBackCheckPoint && canMove)
        {
            canBackCheckPoint = false;
        }

        if (other.CompareTag("Finish"))
        {
            //GameController.instance.Win = true;
        }
    }
}
