// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using UnityEngine;

namespace Coroutine.SimpleCoroutineExample
{
    /// <summary>
    /// Coroutine Example
    /// Ref : https://www.youtube.com/watch?v=RPzQD6D8mcE
    /// </summary>

    public class ScaleExampleWithCoroutine : MonoBehaviour
    {
        [SerializeField] private Transform _objectToScale;
        private UnityEngine.Coroutine _coroutine;
        
        private void Start()
        {
            _coroutine = StartCoroutine(ScalerLoopCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopCoroutine(_coroutine);
            }
        }

        private IEnumerator ScalerLoopCoroutine()
        {
            while (true)
            {
                yield return ScaleCoroutine(_objectToScale.transform.localScale,Vector3.one * 3);
                yield return new WaitForSeconds(2f);
                yield return ScaleCoroutine(_objectToScale.transform.localScale, Vector3.one);
            }
        }

        private IEnumerator ScaleCoroutine(Vector3 startScale, Vector3 endScale)
        {
            float elapsedTime = 0;
            float transitionPercentage = 0;

            while (transitionPercentage <1)
            {
                elapsedTime += Time.deltaTime;
                transitionPercentage = elapsedTime / 2f;
                _objectToScale.localScale = Vector3.Lerp(startScale, endScale, transitionPercentage);
                
                // This line will help us to wait for next frame so while loop wont finish in a single frame !
                yield return null;
            }
        }
    }
}
