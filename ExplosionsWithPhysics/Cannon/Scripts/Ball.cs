using UnityEngine;

namespace ExplosionsWithPhysics.Cannon.Scripts
{
    public class Ball : MonoBehaviour {
        [SerializeField] private Rigidbody _rb;

        public void Init(float velocity) {
            _rb.velocity = transform.forward * velocity;
        }
    }
}
