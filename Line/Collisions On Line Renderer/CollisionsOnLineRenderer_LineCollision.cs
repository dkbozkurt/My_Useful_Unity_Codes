// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Line.Collisions_On_Line_Renderer
{
    /// <summary>
    ///
    /// Attach this script on to gameobject that has linerenderer on it.
    ///
    /// This script works with "CollisionsOnLineRenderer_LineController.cs"
    /// 
    /// Ref : https://www.youtube.com/watch?v=BfP0KyOxVWs
    /// </summary>
    
    [RequireComponent(typeof(CollisionsOnLineRenderer_LineController),typeof(PolygonCollider2D))]
    public class CollisionsOnLineRenderer_LineCollision : MonoBehaviour
    {
        private CollisionsOnLineRenderer_LineController _lineController;
        
        // The collider for the line
        private PolygonCollider2D _polygonCollider2D;
        
        // The points to draw a collision shape between
        // private List<Vector2> _colliderPoints = new List<Vector2>(); 

        private void Awake()
        {
            _lineController = GetComponent<CollisionsOnLineRenderer_LineController>();
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
            
        }

        private void LateUpdate()
        {
            // Get all the positions from the line renderer
            Vector3[] positions = _lineController.GetPositions();
            
            // If we have enough points to draw a line
            if (positions.Count() < 2) _polygonCollider2D.pathCount =0;
            else
            {
                // Get the number of line between two points
                int numberOfLines = positions.Length - 1;
            
                // Make as many paths for each different line as we have lines
                _polygonCollider2D.pathCount = numberOfLines;
            
                // Get collider points between two consecutive points
                for (int i = 0; i < numberOfLines; i++)
                {
                    // Get the two next points
                    List<Vector2> currentPositions = new List<Vector2>
                    {
                        positions[i],
                        positions[i + 1]
                    };

                    List<Vector2> currentColliderPoints = CalculateColliderPoints(currentPositions);
                    _polygonCollider2D.SetPath(i,currentColliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
                }
            }
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.black;
        //     if (_colliderPoints != null)
        //     {
        //         _colliderPoints.ForEach(p => Gizmos.DrawSphere(p,0.1f));
        //     }
        // }

        private List<Vector2> CalculateColliderPoints(List<Vector2> positions)
        {
            // Get the Width of the line
            float width = _lineController.GetWidth();

            // m = (y2 - y1) / (x2 - x1)
            float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
            float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
            float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

            // Calculate the Offset from each point to the collision vertex
            Vector2[] offsets = new Vector2[2];
            offsets[0] = new Vector2(-deltaX, deltaY);
            offsets[1] = new Vector2(deltaX, -deltaY);

            // Generate the Colliders vertices
            List<Vector2> colliderPoints = new List<Vector2>
            {
                positions[0] + offsets[0],
                positions[1] + offsets[0],
                positions[1] + offsets[1],
                positions[0] + offsets[1]

            };

            return colliderPoints;
        }
    }
}