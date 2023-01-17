using System;
using System.Collections.Generic;
using UnityEngine;

namespace Trail_Renderer.Trail_Renderers_with_collisions.Scripts
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailCollisions : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        private EdgeCollider2D _edgeCollider2D;

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();

            // Can be refactored by using Pool
            GameObject colliderGameObject = new GameObject("TrailCollider", typeof(EdgeCollider2D));
            _edgeCollider2D = GetComponent<EdgeCollider2D>();
        }

        private void Update()
        {
            SetColliderPointsFromTrail(_trailRenderer, _edgeCollider2D);
        }

        private void SetColliderPointsFromTrail(TrailRenderer trail,EdgeCollider2D collider)
        {
            List<Vector2> points = new List<Vector2>();

            for (int position = 0; position < trail.positionCount; position++)
            {
                points.Add(trail.GetPosition(position));
            }

            collider.SetPoints(points);
        }

        private void OnDestroy()
        {
            Destroy(_edgeCollider2D.gameObject);
        }
    }
}
