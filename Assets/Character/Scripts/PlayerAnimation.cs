using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator AnimatorPlayer => animator;
        public void PlayAnimRun(float speed)
        {
            animator.SetFloat("Speed", Mathf.Abs(speed));
        }

        public void PlayAnimJump(bool jumpping)
        {
            animator.SetBool("IsJumping", jumpping);
        }

        private bool isFall;
        public void PlayAnimIsFall()
        {
            if (!isFall)
            {
                isFall = true;
                animator.SetTrigger("IsFall");
                DOVirtual.DelayedCall(1.2f, delegate
                {
                    isFall = false;
                });
            }
        }
        public void SetSpeedCurrentAnimation(float speed)
        {
            animator.speed = speed;
        }

    }
}