using UnityEngine;

namespace ExplosionsWithPhysics.Cannon.Scripts
{
    public class Cannon : MonoBehaviour {
        [SerializeField] private Ball _ballPrefab;

        [SerializeField] private Transform _ballSpawn;

        [SerializeField] private float _velocity = 10;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                var ball = Instantiate(_ballPrefab, _ballSpawn.position, _ballSpawn.rotation);
                ball.Init(_velocity);
            }
        }
    }
}
