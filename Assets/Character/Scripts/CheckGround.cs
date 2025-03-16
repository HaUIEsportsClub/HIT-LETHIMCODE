using System;
using UnityEngine;

namespace Character
{
    public class CheckGround : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        private bool isGround;
        public bool GroundChecked()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position - Vector3.up * 0.1f, 0.1f, Vector2.down, 0f,groundLayer );
            return hit.collider != null;
            /*return isGround;*/
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ground") && PlayerController.Instance.Skill.CanBackCheckPoint && PlayerController.Instance.CanMove)
            {
                PlayerController.Instance.Skill.CanBackCheckPoint = false;
            }

            if (other.CompareTag("Finish"))
            {
                //GameController.instance.Win = true;
            }

            if (other.CompareTag("Losse"))
            {
                //Lo
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
            Vector3 circleCenter = transform.position - Vector3.up * 0.1f;
            float radius = 0.1f;
            Gizmos.DrawWireSphere(circleCenter, radius);
        }
    }
}