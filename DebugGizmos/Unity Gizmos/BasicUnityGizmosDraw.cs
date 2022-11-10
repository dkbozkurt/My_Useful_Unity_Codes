// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DebugGizmos.Unity_Gizmos
{
    /// <summary>
    /// 
    /// Ref : https://www.youtube.com/watch?v=4Osn0g_Y24k&ab_channel=Tarodev
    /// </summary>
    public class BasicUnityGizmosDraw : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            // DrawSpheres();
            // DrawLoopSpheres();
            // DrawRay();
        }

        private void DrawSpheres()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Vector3.zero,0.5f);
            Gizmos.DrawWireSphere(Vector3.one, 0.5f);    
        }

        private void DrawLoopSpheres()
        {
            for (int i = 0; i < 1000; i++)
            {
                Gizmos.color = Random.ColorHSV();
                Gizmos.DrawSphere(Random.insideUnitSphere* 5,0.5f);
            }
        }

        private void DrawRay()
        {
            var multiplier = 10;
            Gizmos.DrawRay(Vector3.zero, Vector3.one *  multiplier);
        }
    }
}
