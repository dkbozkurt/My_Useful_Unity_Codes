// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DebugGizmos.Unity_Gizmos
{
    /// <summary>
    ///
    /// Edited script, which supports to work in 3D environment
    ///  
    ///  Ref : https://www.youtube.com/watch?v=4Osn0g_Y24k&ab_channel=Tarodev
    /// </summary>
    public class BezierCurveByGizmos : MonoBehaviour
    {
        [SerializeField, Range(4, 200)] private int _resolution = 50;
        [SerializeField] private List<Transform> _controlPoints;
        [SerializeField] private Vector3[] _points;

        [SerializeField] private bool _drawAnchors, _drawResolutionPoints, _drawConnections;

        private void OnDrawGizmos()
        {
            float segmentLength = 1 / (float) _resolution;

            // Create main lines
            var sets = new List<Vector3[]>();
            for (int i = 0; i < 2; i++)
            {
                var set = new Vector3[_resolution];
                for (int j = 0; j < _resolution; j++)
                {
                    set[j] = Vector3.Lerp(_controlPoints[i].position, _controlPoints[i + 1].position,
                        segmentLength * (j + 1));
                }
                
                sets.Add(set);
            }
            
            // Add bezier points
            _points = new Vector3[_resolution];
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _resolution; i++)
            {
                _points[i] = Vector3.Lerp(sets[0][i], sets[1][i], segmentLength * i);
                if(_drawResolutionPoints) Gizmos.DrawSphere(_points[i],0.08f);
            }
            
            // Draw bezier line
            Gizmos.color = Color.green;
            for (int i = 1; i < _resolution; i++)
            {
                Gizmos.DrawLine(_points[i-1],_points[i]);
            }
            
            if(_drawAnchors) DrawAnchors();
            if (_drawConnections) DrawConnections();
        }
        

        private void DrawAnchors()
        {
            Gizmos.color = Color.red;
            foreach (var anchor in _controlPoints)
            {
                Gizmos.DrawSphere(anchor.position,0.2f);
            }
        }

        private void DrawConnections()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_controlPoints[0].position,_controlPoints[1].position);
            Gizmos.DrawLine(_controlPoints[1].position,_controlPoints[2].position);
        }
    }
}
