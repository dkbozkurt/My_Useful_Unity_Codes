using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slerp.SlerpBasics
{
    public class EvaluateSlerp : MonoBehaviour
    {
        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;
        [SerializeField] private Transform centerOffSetTransform;

        private void Update()
        {
            EvaluateSlerpPoints(Vector3.zero, new Vector3(5, 0), -2.1261f);
        }

        IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, float centerOffSet)
        {
            var centerPivot = (start + end) * 0.5f;

            centerPivot -= new Vector3(0, -centerOffSet);

            var startRelativeCenter = start - centerPivot;
            var endRelativeCenter = end - centerPivot;

            var f = 1f / 10;

            for (var i = 0f; i < 1 + f; i += f)
            {
                yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, 1) + centerPivot;
            }
        }
    }
}
