// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using UnityEngine;

namespace GeneralPhysics.SlowDownByClampingVelocity
{
    [RequireComponent(typeof(Rigidbody))]
    public class SlowDownByClampingVelocity : MonoBehaviour
    {
        [SerializeField] private float _decelerationMultiplier = 10f;
        
        private Rigidbody _rigidbody;
        
        private bool _stopInProgress = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                SpeedUp();
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                StopOverTime(_decelerationMultiplier);
            }
        }

        private void SpeedUp()
        {
            if (_rigidbody.velocity.magnitude <= 0)
            {
                _rigidbody.AddForce(Vector3.up* 2f,ForceMode.Impulse);
            }
            _rigidbody.velocity *= 1.2f;
        }

        private void StopOverTime(float decelerationMultiplier)
        {
            if(_stopInProgress) return;

            _stopInProgress = true;

            StartCoroutine(Stop());

            IEnumerator Stop()
            {
                while (true)
                {
                    var velocity = _rigidbody.velocity;
                    velocity.x = Mathf.Clamp(velocity.x -= Time.deltaTime * decelerationMultiplier, 0, float.MaxValue);
                    velocity.y = Mathf.Clamp(velocity.y -= Time.deltaTime * decelerationMultiplier, 0, float.MaxValue);
                    velocity.z = Mathf.Clamp(velocity.z -= Time.deltaTime * decelerationMultiplier, 0, float.MaxValue);
                    _rigidbody.velocity = velocity;

                    if (velocity.magnitude <= 0)
                    {
                        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                        yield break;
                    }

                    yield return null;
                }
            }
            
        }
    }
}
