// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace Inverse_Kinematics
{
    /// <summary>
    /// Head LookAt with using IK
    ///
    /// Attach this script onto player's component with Animator on it.
    /// Note : First thing to do is enable IK Pass in the Layer settings of the player's Animator.
    /// 
    /// Ref : https://www.youtube.com/watch?v=SLAYPZ7lukY
    /// </summary>
    
    [RequireComponent(typeof(Animator))]
    public class HeadLookAtUsingIK : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float maxDistanceToTrackObject;
        
        [Space]
        [SerializeField] private float lookWeight;
        [SerializeField] private float lookingInterval;
        [SerializeField] private float characterHeight;
        
        public bool iKActive = false;

        #region Dummy Pivot

        private GameObject _objectPivot;

        #endregion

        private Animator _animator;
        private float _distance;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            _objectPivot = new GameObject("DummyPivot");
            _objectPivot.transform.parent = transform.parent;
            _objectPivot.transform.localPosition = new Vector3(0,characterHeight,0);
        }

        private void Update()
        {
            TrackObject();
        }

        private void TrackObject()
        {
            _objectPivot.transform.LookAt(targetTransform);
            float pivotRotationY = _objectPivot.transform.localRotation.y;
            _distance = Vector3.Distance(_objectPivot.transform.position, targetTransform.position);
            if (pivotRotationY < lookingInterval && pivotRotationY > -lookingInterval && _distance < maxDistanceToTrackObject)
            {
                // Target tracking
                lookWeight = Mathf.Lerp(lookWeight,1,Time.deltaTime*2.5f);
            }
            else
            {
                // Target Release
                lookWeight = Mathf.Lerp(lookWeight,0,Time.deltaTime*2.5f);
            }
        }

        private void OnAnimatorIK(int layerIndex = 0)
        {
            if (!_animator) return;

            if (iKActive)
            {
                if(!targetTransform) return;
                
                _animator.SetLookAtWeight(lookWeight);
                _animator.SetLookAtPosition(targetTransform.position);
            }
            else
            {
                _animator.SetLookAtWeight(0);
            }
        }
    }
}
