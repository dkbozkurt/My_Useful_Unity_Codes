// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Line.Collisions_On_Line_Renderer
{
    /// <summary>
    ///
    /// Attach this script on to gameobject that has linerenderer on it.
    ///
    /// This script works with "CollisionsOnLineRenderer_LineCollision.cs"
    /// Ref : https://www.youtube.com/watch?v=BfP0KyOxVWs
    /// </summary>
    
    [RequireComponent(typeof(LineRenderer))]
    public class CollisionsOnLineRenderer_LineController : MonoBehaviour
    {
        [SerializeField] private List<Transform> nodes;
        [SerializeField] private float depthValue = 5;
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = nodes.Count;
        }

        private void Update()
        {
            SetPositions();
        }

        private void SetPositions()
        {
            _lineRenderer.SetPositions(nodes.ConvertAll(n => n.position - new Vector3(0,0,depthValue)).ToArray());
        }

        public Vector3[] GetPositions()
        {
            Vector3[] positions = new Vector3[_lineRenderer.positionCount];
            _lineRenderer.GetPositions(positions);
            return positions;
        }

        public float GetWidth()
        {
            return _lineRenderer.startWidth;
        }
    }
}
