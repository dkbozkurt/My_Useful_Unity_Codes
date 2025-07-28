using UnityEngine;

namespace DkbozkurtAStarPLY.Scripts
{
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class Obstacle : MonoBehaviour, IObstacle
    {
        public bool IsValid { get; set; }

        private void Awake()
        {
            IsValid = false;
        }
    }
}
