// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Line.Draw_Line_To_Clicked_Point_And_Generate_Shapes
{
    /// <summary>
    /// Draw Line from point to point pen tool
    ///
    /// For 2D space. Camera need to be orthographic.
    ///  
    /// Ref : https://www.youtube.com/watch?v=pcLn2ze9JQA
    /// </summary>
    
    public class DrawLineToClickPointAndGenerateShapes_PenTool : MonoBehaviour
    {
        [Header("Pen Canvas")] 
        [SerializeField] private DrawLineToClickPointAndGenerateShapes_PenCanvas penCanvas;
        
        [Header("Data")] 
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Transform dotParent;

        [Header("Lines")] 
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private Transform lineParent;
        private DrawLineToClickPointAndGenerateShapes_LineController _currentLine;

        [Header("Loop Toggle")]
        [SerializeField] private Image loopToggle;
        [SerializeField] private Sprite loopSprite;
        [SerializeField] private Sprite unloopSprite;

        private void Start()
        {
            penCanvas.OnPenCanvasLeftClickEvent += AddDot;
            penCanvas.OnPenCanvasRightClickEvent += EndCurrentLine;
        }

        private void EndCurrentLine()
        {
            if(!_currentLine) return;

            _currentLine = null;

        }

        private void OnDisable()
        {
            penCanvas.OnPenCanvasLeftClickEvent -= AddDot;
        }

        public void ToggleLoop()
        {
            if (!_currentLine) return;

            _currentLine.ToggleLoop();
            loopToggle.sprite = (_currentLine.isLooped()) ? unloopSprite : loopSprite;
        }
        private void AddDot()
        {
            if (!_currentLine)
            {
                _currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent)
                    .GetComponent<DrawLineToClickPointAndGenerateShapes_LineController>();
            }

            DrawLineToClickPointAndGenerateShapes_DotController dot = Instantiate(dotPrefab, GetMousePosition(), Quaternion.identity, dotParent).GetComponent<DrawLineToClickPointAndGenerateShapes_DotController>();
            dot.OnDragEvent += MoveDot;
            dot.OnRightClickEvent += RemoveDot;
            dot.OnLeftClickEvent += SetCurrentLine;

            _currentLine.AddPoint(dot);
        }

        private void SetCurrentLine(DrawLineToClickPointAndGenerateShapes_DotController dot)
        {
            EndCurrentLine();
            _currentLine = dot.lineController;
        }

        private void MoveDot(DrawLineToClickPointAndGenerateShapes_DotController dot)
        {
            dot.OnDragEvent -= MoveDot;
            dot.transform.position = GetMousePosition();
        }

        private void RemoveDot(DrawLineToClickPointAndGenerateShapes_DotController dot)
        {
            dot.OnRightClickEvent -= RemoveDot;
            DrawLineToClickPointAndGenerateShapes_LineController line = dot.lineController;
            line.SplitPointAtIndex(dot.index,out List<DrawLineToClickPointAndGenerateShapes_DotController> before, out List<DrawLineToClickPointAndGenerateShapes_DotController> after);
            
            Destroy(line.gameObject);
            Destroy(dot.gameObject);

            DrawLineToClickPointAndGenerateShapes_LineController beforeLine =
                Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent)
                    .GetComponent<DrawLineToClickPointAndGenerateShapes_LineController>();
            for (int i = 0; i < before.Count; i++)
            {
                beforeLine.AddPoint(before[i]);
            }
            
            DrawLineToClickPointAndGenerateShapes_LineController afterLine =
                Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent)
                    .GetComponent<DrawLineToClickPointAndGenerateShapes_LineController>();
            for (int i = 0; i < after.Count; i++)
            {
                afterLine.AddPoint(after[i]);
            }
        }

        private Vector3 GetMousePosition()
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition.z = 0;
            
            return worldMousePosition;
        }
    }
}
