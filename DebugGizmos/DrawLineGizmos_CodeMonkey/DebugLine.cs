// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace DebugGizmos.CodeMonkey
{
    /// <summary>
    /// If you want to see line in game mode you should enable gizmos of the "GAME" window.
    ///  
    /// Ref : https://www.youtube.com/watch?v=15n-ilpYqME&ab_channel=CodeMonkey
    /// </summary>
    public class DebugLine : MonoBehaviour
    {
        [SerializeField] private float _debugLineVanishDuration = 100f;

        private void Start()
        {
            DrawGizmoLine(Vector3.zero, Vector3.one, Color.white, _debugLineVanishDuration);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = GetMouseWorldPosition();
                DrawGizmoLine(Vector3.zero,mousePosition,Color.cyan);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                DrawPath();
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DrawQuadrant(0,0);
                DrawQuadrant(1,0);
                DrawQuadrant(0,1);
                DrawQuadrant(1,1);
            }
        }

        private void DrawPath()
        {
            Vector3[] path = new Vector3[]
            {
                new Vector3(0, 0),
                new Vector3(10, 10),
                new Vector3(20, 10),
                new Vector3(20, 20),
                new Vector3(40,20)
            };

            for (int i = 0; i < path.Length-1; i++)
            {
                DrawGizmoLine(path[i],path[i+1],Color.yellow);
            }

        }
        
        private void DrawQuadrant(int x,int y)
        {
            float quadrantSize = 10f;
            
            DrawGizmoLine(new Vector3(x+0,y+0) * quadrantSize, new Vector3(x+1,y+0)* quadrantSize,Color.gray);
            DrawGizmoLine(new Vector3(x+0,y+1) * quadrantSize, new Vector3(x+1,y+1)* quadrantSize,Color.gray);
            DrawGizmoLine(new Vector3(x+1,y+0) * quadrantSize, new Vector3(x+1,y+1)* quadrantSize,Color.gray);
            DrawGizmoLine(new Vector3(x+0,y+0) * quadrantSize, new Vector3(x+0,y+1)* quadrantSize,Color.gray);
        }
        
        private void DrawGizmoLine(Vector3 startPosition,Vector3 endPosition, Color color, float duration=5)
        {
            Debug.DrawLine(startPosition,endPosition,color,duration);
        }

        #region Get Mouse Position in World with Z = 0f

        public Vector3 GetMouseWorldPosition()
        {
            Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vector.z = 0f;
            return vector;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition,Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        #endregion
    }
}
