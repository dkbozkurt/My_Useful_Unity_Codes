// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Lerp.TheRightWayToLerp
{
    /// <summary>
    /// Works in a similar way to lerp in that it changes a value towards a target
    /// in a linear scale but it's controlled by speed not time.
    /// 
    /// Ref : https://www.youtube.com/watch?v=RNccTrsgO9g&ab_channel=GameDevBeginner
    /// </summary>
    public class MoveTowardsToLerp : MonoBehaviour
    {
        [SerializeField] private float _lerpSpeed = 1f;
        private float _valueToLerp = 1f;

        private void Update()
        {
            LerpTheValueWithMoveTowards(3f,_lerpSpeed);
        }

        private void LerpTheValueWithMoveTowards(float target,float lerpSpeed)
        {
            _valueToLerp = Mathf.MoveTowards(_valueToLerp, target, lerpSpeed * Time.deltaTime);
        }
        
    }
}