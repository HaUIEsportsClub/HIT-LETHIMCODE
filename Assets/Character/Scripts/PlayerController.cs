﻿using pooling;
using UnityEngine;

namespace Character
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Requiement")] 
        [SerializeField]
        private PlayerMovement movement;
        public PlayerMovement PlayerMovement => movement;

        [SerializeField] 
        private PlayerAnimation animationPlayer;
        public PlayerAnimation AnimationPlayer => animationPlayer;

        [SerializeField] 
        private CheckGround groundChecked;
        public CheckGround GroundChecked => groundChecked;
        
        [SerializeField] 
        private SpecialSkill specialSkill;
        public SpecialSkill Skill => specialSkill;

        [SerializeField] 
        private ChangeColorPlayer changeColorPlayer;
        
        [Header("Setting")]
        [Header("Setting Movement")]
        private float horizontal;
        [SerializeField] private float speedMove = 5f;
        [SerializeField] private float jumpForce = 8f;
        private bool canMove = true;
        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }

        private bool canJump = true;
        public bool CanJump
        {
            get => canJump;
            set => canJump = value;
        }

        [Header("Setting Collider 2D")]
        [SerializeField]
        private Collider2D collider2D;
        public Collider2D Collider2DPlayer => collider2D;

        [Header("Setting Layer")] 
        [SerializeField]
        private LayerMask layerMap;
        protected override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        }
        
        private void Update()
        {
            if(GameController.Instance.State != GameController.StateGame.Playing) return;
            if (isCheckMovePlay && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetKeyDown(KeyCode.W)))
            {
                isCheckMovePlay = false;
                ToggleMovementState(true);
            }
            if (!canMove) return;
            horizontal = Input.GetAxisRaw("Horizontal");
            animationPlayer.PlayAnimRun(horizontal);
            movement.Move(horizontal, speedMove);
            if (groundChecked.GroundChecked() && !canJump)
            {
                animationPlayer.PlayAnimJump(false);
                canJump = true;
            }
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && groundChecked.GroundChecked() && canJump)
            {
                animationPlayer.PlayAnimJump(true);
                movement.Jump(jumpForce);
                canJump = false;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Skill.SkillReturnSavePoint();
            }
        }
        private void FixedUpdate()
        {
            VerifyStableLanding();
            if (!canMove) return;
            SetParentPlayer();
        }
        private void SetParentPlayer()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, Vector2.down, 0f, layerMap);
            if (hit.collider != null)
            {
                this.transform.SetParent(hit.transform);
                changeColorPlayer.ChangeColor(hit.transform.GetChild(0).name);
            }
        }

        private bool isCheckMovePlay = false;

        private void VerifyStableLanding()
        {
            RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, Vector2.down, 0.5f);
            if (hit2D.Length >= 4)
            {
                ToggleMovementState(false);
                isCheckMovePlay = true;
            }
        }

        private bool isFalling;
        
        public void ToggleMovementState(bool enable)
        {
            if (isCheckMovePlay) return;
            movement.StopMove(enable);
            canMove = enable;
            collider2D.enabled = enable;
            animationPlayer.SetSpeedCurrentAnimation(enable? 1f:0f);
        }
    }
}
