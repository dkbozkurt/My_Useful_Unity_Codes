// Dogukan Kaan Bozkurt
// github.com/dkbozkurt

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DrawLineRenderersWithPhysics
{
    /// <summary>
    /// Draw Line Renderers with Physics
    /// 
    /// Ref : https://www.youtube.com/watch?v=KojYeZwEPyQ&ab_channel=HamzaHerbou
    /// </summary>

    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Line : MonoBehaviour
    {
        [HideInInspector] public List<Vector2> points = new List<Vector2>();
        [HideInInspector] public int pointsCount = 0;

        private LineRenderer _lineRenderer;
        private EdgeCollider2D _edgeCollider;
        private Rigidbody2D _rigidbody;
        
        // Min distance between line's points
        private float _pointsMinDistance = 0.1f;

        private float _circlesColliderRadius;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _edgeCollider = GetComponent<EdgeCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        // If distance between last point and new point is less than MinPointsDistance,
        // don't draw the point (skip)
        public void AddPoint(Vector2 newPoint)
        {
            if (pointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < _pointsMinDistance)
                return;
            points.Add(newPoint);
            pointsCount++;

            // Add Circle collider to the point
            CircleCollider2D circleCollider = this.gameObject.AddComponent<CircleCollider2D>();
            circleCollider.offset = newPoint;
            circleCollider.radius = _circlesColliderRadius;
            
            // Line Renderer
            _lineRenderer.positionCount = pointsCount;
            _lineRenderer.SetPosition(pointsCount-1,newPoint);

            // Edge Collider
            // (Edge Collider accepts only 2 or more points)
            if (pointsCount > 1)
                _edgeCollider.points = points.ToArray();
        }

        public Vector2 GetLastPoint()
        {
            return (Vector2)_lineRenderer.GetPosition(pointsCount - 1);
        }

        public void UsePhysics(bool usePhysics)
        {
            _rigidbody.isKinematic = !usePhysics;
        }

        public void SetLineColor(Gradient colorGradient)
        {
            _lineRenderer.colorGradient = colorGradient;
        }

        public void SetPointsMinDistance(float distance)
        {
            _pointsMinDistance = distance;
        }

        public void SetLineWidth(float width)
        {
            _lineRenderer.startWidth = width;
            _lineRenderer.endWidth = width;

            _circlesColliderRadius = width / 2f;
            _edgeCollider.edgeRadius = _circlesColliderRadius;
        }
    }
}
