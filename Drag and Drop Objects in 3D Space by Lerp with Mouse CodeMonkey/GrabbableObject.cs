// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    /// <summary>
    /// 
    /// Set grabbable objects, rigidbodys interpolate value to -> Interpolate
    /// Set some drag value to grabbable objects Rigidbody, like '5' to avoid trembling effect
    /// which occurs when grabbable object pushed by another collider.
    /// 
    /// Ref : https://www.youtube.com/watch?v=2IhzPTS4av4&ab_channel=CodeMonkey
    /// </summary>
    [RequireComponent(typeof(Collider),typeof(Rigidbody))]
    public class GrabbableObject : MonoBehaviour
    {
        [SerializeField] private bool makeKinematicWhenGrabbed = false;
        private Rigidbody _objectRigidbody;
        private Transform _objectGrabPointTransform;
        private float _lerpSpeed = 10f;

        private void Awake()
        {
            _objectRigidbody = GetComponent<Rigidbody>();
        }

        public void Grab(Transform grabPointTransform)
        {
            if (makeKinematicWhenGrabbed) _objectRigidbody.isKinematic = true;
            _objectGrabPointTransform = grabPointTransform;
            _objectRigidbody.useGravity = false;
        }

        public void Drop()
        {
            if (makeKinematicWhenGrabbed) _objectRigidbody.isKinematic = false;
            _objectGrabPointTransform = null;
            _objectRigidbody.useGravity = true;
        }

        private void FixedUpdate()
        {
            if(_objectGrabPointTransform == null) return;
            Vector3 newPosition = Vector3.Lerp(transform.position, _objectGrabPointTransform.position,_lerpSpeed * Time.deltaTime);
            _objectRigidbody.MovePosition(newPosition);
        }
    }
}
