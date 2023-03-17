// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using UnityEngine;

namespace Lerp.TheRightWayToLerp
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=RNccTrsgO9g&ab_channel=GameDevBeginner
    /// </summary>
    public class LerpWithCoroutine : MonoBehaviour
    {
        [Header("- Core Settings -")]
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.Linear(0f,0f,1f,1f);
        [SerializeField] private float _lerpDuration =3f;
        
        [Header("- LerpFloatCoroutine -")]
        [SerializeField] private float _initialValue = 0f;
        [SerializeField] private float _endValue = 3f;
        private float _valueToLerp;
        

        [Header("- LerpVector3Coroutine - ")]
        [SerializeField] private Vector3 _positionA = Vector3.zero;
        [SerializeField] private Vector3 _positionB = Vector3.one;
        private Vector3 _targetPosition;

        [Header("- LerpColorCoroutine - ")]
        [SerializeField] private SpriteRenderer _targetSprite;
        [SerializeField] private Color _initialColor = Color.white;
        [SerializeField] private Color _endColor = Color.black;

        private void Start()
        {
            LerpTheValue();
        }

        private void LerpTheValue()
        {
            // StartCoroutine(LerpFloatCoroutine(_initialValue,_endValue,_lerpDuration));
            // StartCoroutine(LerpVector3Coroutine(_positionA,_positionB,_lerpDuration));
            // StartCoroutine(LerpColorCoroutine(_initialColor, _endColor, _lerpDuration));
        }

        private IEnumerator LerpFloatCoroutine(float startValue, float endValue,float duration = 1f)
        {
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;

                t = _animationCurve.Evaluate(t);
                //t = t * t * (3f - 2f * t);
                
                _valueToLerp = Mathf.Lerp(startValue, endValue, t);
                timeElapsed += Time.deltaTime;
                
                yield return null;
            }

            _valueToLerp = endValue;
        }
        
        private IEnumerator LerpVector3Coroutine(Vector3 startValue, Vector3 endValue,float duration = 1f)
        {
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;

                t = _animationCurve.Evaluate(t);
                //t = t * t * (3f - 2f * t);
                
                _targetPosition = Vector3.Lerp(startValue, endValue, t);
                timeElapsed += Time.deltaTime;
                
                yield return null;
            }

            _targetPosition = endValue;
        }
        
        private IEnumerator LerpColorCoroutine(Color startValue, Color endValue,float duration = 1f)
        {
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;

                
                t = _animationCurve.Evaluate(t);
                //t = t * t * (3f - 2f * t);
                
                _targetSprite.color = Color.Lerp(startValue, endValue, t);
                timeElapsed += Time.deltaTime;
                
                yield return null;
            }

            _targetSprite.color = endValue;
        }
        
        
    }
}