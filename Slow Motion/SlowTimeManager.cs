// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Slow_Motion
{
    /// <summary>
    /// Create Explosion at the pointed location.
    ///
    /// Ref : https://www.youtube.com/watch?v=0VGosgaoTsw
    /// </summary>
    
    public class SlowTimeManager : MonoBehaviour
    {
        [SerializeField] private float slowdownFactor = 0.05f;
        [SerializeField] private float slowdownLength = 2f;

        private void Update()
        {
            SetTimeToNormalScale();
        }

        public void DoSlowMotion()
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

        private void SetTimeToNormalScale()
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }
}
 