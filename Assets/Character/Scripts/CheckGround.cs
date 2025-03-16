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
            /*float extraHeight = 0.1f;
            Vector2 boxSize = new Vector2(0.8f, 0.8f);
            Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f);

            RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, extraHeight, groundLayer);

            Debug.DrawRay(origin, Vector2.down * extraHeight, Color.red);

            return hit.collider != null;*/
            return isGround;
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
            float radius = 0.1f;
            Gizmos.DrawWireSphere(circleCenter, radius);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGround = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGround = false;
            }
        }
    }
}