// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Animation_Rigging
{
    /// <summary>
    /// Animation Rigging
    ///
    /// !!! YOU MUST IMPORT "ANIMATION RIGGING" PACKAGE FROM PACKAGE MANAGER !!!
    /// 
    /// Ref : https://www.youtube.com/watch?v=LEwYmFT3xDk
    /// </summary>

    [RequireComponent(typeof(Rig))]
    public class LookAtObjectAnimationRigging : MonoBehaviour
    {
        private Rig _rig;
        private float _targetWeight;
        private void Awake()
        {
            GetComponent<Rig>();
        }

        private void Update()
        {
            LookAtTarget();

            if (Input.GetKeyDown(KeyCode.T))
            {
                _targetWeight = 1f;
            }
            
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _targetWeight = 0f;
            }
        }

        private void LookAtTarget()
        {
            // To get Smooth Interpolation we used Lerp.
            _rig.weight = Mathf.Lerp(_rig.weight, _targetWeight, Time.deltaTime * 10f);
        }
    }
}
