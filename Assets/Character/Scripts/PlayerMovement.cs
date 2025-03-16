using UnityEngine;

namespace Character
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        public Rigidbody2D Rigidbody2D => rb;
        public void Move(float direction, float moveSpeed)
        {
            if (direction != 0)
            {
                Flip(direction);
            }
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        }

        public void StopMove(bool enable)
        {
            rb.bodyType = enable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
        }
        private void Flip(float direction)
        {
            if (direction != 0)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1 : -1);
                transform.localScale = scale;
            }
        }
        public void Jump(float jumpForce)
        {
            rb.velocity = Vector2.up * jumpForce * Mathf.Sign(rb.gravityScale);
        }
    }
}