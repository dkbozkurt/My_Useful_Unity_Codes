// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Line.Draw_Line_Between_Points
{
    /// <summary>
    ///
    /// Attach this script onto gameobject with line renderer component.
    ///
    /// Use this script with "PointToPointLineDrawManager.cs"
    /// 
    /// Ref : https://www.youtube.com/watch?v=5ZBynjAsfwI
    /// </summary>
    
    [RequireComponent(typeof(LineRenderer))]
    public class PointToPointLineDrawer : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private Transform[] _points;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            for (int i = 0; i < _points.Length; i++)
            {
                _lineRenderer.SetPosition(i,_points[i].position);
            } 
        }

        public void SetUpLine(Transform[] points)
        {
            _lineRenderer.positionCount = points.Length;
            this._points = points;
        }
        
    }
}
