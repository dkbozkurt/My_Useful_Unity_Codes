// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace GeneralPhysics.ThrowObjectFromPointToPoint
{
    /// <summary>
    /// Object to Throw must have COLLIDER AND RIGIDBODY.
    ///
    /// Ref : https://www.youtube.com/watch?v=F20Sr5FlUlE&ab_channel=Dave%2FGameDevelopment
    /// </summary>
    public class ThrowObjectFromPointToPoint : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTrasform;

        [SerializeField] private float _upwardForceMultiplier = 2.5f;
        [SerializeField] private float _forwardForceMultiplier = 8f;
        
        public void ThrowObject(GameObject objectToThrow,Transform targetTransform)
        {
            Rigidbody objectRigidbody = objectToThrow.GetComponent<Rigidbody>();
            Vector3 forceDirection = targetTransform.position - _spawnTrasform.position;
            forceDirection = forceDirection.normalized;
            Vector3 forceToAdd = forceDirection * _forwardForceMultiplier + transform.up * _upwardForceMultiplier;
            objectRigidbody.AddForce(forceToAdd,ForceMode.Impulse);
        }
    }
}
