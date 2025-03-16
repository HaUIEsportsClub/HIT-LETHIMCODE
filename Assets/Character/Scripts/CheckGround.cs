using System;
using UnityEngine;

namespace Character
{
    public class CheckGround : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        private bool isGround;

        private const float radiusCircle = 0.35f;

        public bool GroundChecked()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, radiusCircle, Vector2.down, 0f,
                groundLayer);

            isGround = hit.collider != null;
            return isGround;
            /*return isGround;*/
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground") && PlayerController.Instance.Skill.CanBackCheckPoint &&
                PlayerController.Instance.CanMove)
            {
                PlayerController.Instance.Skill.CanBackCheckPoint = false;
            }

            if (other.CompareTag("Finish"))
            {
                GameController.Instance.Win();
            }

            if (other.CompareTag("Losse"))
            {
                GameController.Instance.Replay();
            }
        }

        public void PlayAnimFalling()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.1f, Vector2.down, 0f, groundLayer);
            if (hit.collider == null)
            {
                PlayerController.Instance.AnimationPlayer.PlayAnimIsFall();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 circleCenter = transform.position;
            Gizmos.DrawWireSphere(circleCenter, radiusCircle);
        }
    }
}