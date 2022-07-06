// Dogukan Kaan Bozkurt
// github.com/dkbozkurt

using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DrawLineRenderersWithPhysics
{
    /// <summary>
    /// Draw Line Renderers with Physics
    ///
    /// This script must work with "Line.cs" script.
    ///
    /// Add Layer named as "CantDrawOver" to avoid drawing on scene objects and
    /// over currently drawing line.
    /// 
    /// Ref : https://www.youtube.com/watch?v=KojYeZwEPyQ&ab_channel=HamzaHerbou
    /// </summary>
    
    public class LinesDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private LayerMask cantDrawOverLayer;
        private int cantDrawOverLayerIndex;

        [Space(30f)] 
        [SerializeField] private Gradient lineColor;
        [SerializeField] private float linePointsMinDistance;
        [SerializeField] private float lineWidth;

        private Camera _camera;

        private Line _currentLine;

        private void Start()
        {
            _camera = Camera.main;
            cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                BeginDraw();
            }

            if (_currentLine != null)
            {
                Draw();
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                EndDraw();
            }
        }

        private void BeginDraw()
        {
            _currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();
            
            _currentLine.UsePhysics(false);
            _currentLine.SetLineColor(lineColor);
            _currentLine.SetPointsMinDistance(linePointsMinDistance);
            _currentLine.SetLineWidth(lineWidth);
        }

        private void Draw()
        {
            Vector2 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
            
            if(hit) EndDraw();
            else _currentLine.AddPoint(mousePosition);
            
        }

        private void EndDraw()
        {
            if(_currentLine == null) return;

            if (_currentLine.pointsCount < 2)
            {
                Destroy(_currentLine.gameObject);
            }
            else
            {
                _currentLine.gameObject.layer = cantDrawOverLayerIndex;
                _currentLine.UsePhysics(true);
                _currentLine = null;
            }
        }
    }
}