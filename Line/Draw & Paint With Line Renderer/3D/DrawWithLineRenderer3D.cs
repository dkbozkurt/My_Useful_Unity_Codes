// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;

namespace Line.Draw___Paint_With_Line_Renderer._3D
{
    /// <summary>
    /// Draw in Unity with Line Renderer in 3D Space
    /// 
    /// Attach this script onto empty GameObject and assign values
    ///
    /// NOTE : Camera Projection must be Perspective.
    /// 
    /// Ref : https://www.youtube.com/watch?v=mgmfZRllpzs&ab_channel=contour3D
    /// </summary>
    public class DrawWithLineRenderer3D : MonoBehaviour
    {
        private Camera _mainCamera;
        private List<Vector3> _linePoints = new List<Vector3>();

        [SerializeField] private float timerDelay = 0.02f;
        private float _timer;

        #region Line Properties

        [SerializeField] private float lineWidth = 0.1f;
        [SerializeField] private Color lineColor = Color.red;
        private GameObject _newLine;
        private LineRenderer _drawLine;
        
        // If isPlayable
        [SerializeField] private bool isPlayable = false;
        [SerializeField] private LineRenderer drawLinePrefab = null;

        #endregion

        private float _rayDepth = 10f;


        private void Awake()
        {
            _mainCamera = Camera.main;
            _mainCamera.orthographic = false;
            _timer = timerDelay;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                AssignInitialLineValues();
            }
            
            if (Input.GetMouseButton(0))
            {
                DrawRayFromCamera();
                WaitTimeToAddPoint();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _linePoints.Clear();
            }
        }

        private void AssignInitialLineValues()
        {
            if (isPlayable)
            {
                _drawLine = Instantiate(drawLinePrefab, drawLinePrefab.transform.position, Quaternion.identity);
            }
            else
            {
                _drawLine = new GameObject().AddComponent<LineRenderer>();
                _drawLine.material = new Material(Shader.Find("Sprites/Default"));    
            }
            _drawLine.startColor = lineColor;
            _drawLine.endColor = lineColor;
            _drawLine.startWidth = lineWidth;
            _drawLine.endWidth = lineWidth;

        }

        private void DrawLine()
        {
            _drawLine.positionCount = _linePoints.Count;
            _drawLine.SetPositions(_linePoints.ToArray());
        }

        private void WaitTimeToAddPoint()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _linePoints.Add(GetMousePosition());
                DrawLine();
                _timer = timerDelay;
            }
        }

        private Vector3 GetMousePosition()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            return ray.origin + ray.direction * _rayDepth;
        }

        private void DrawRayFromCamera()
        {
            Debug.DrawRay(_mainCamera.ScreenToWorldPoint(Input.mousePosition), GetMousePosition(), Color.red);
        }
    }
}