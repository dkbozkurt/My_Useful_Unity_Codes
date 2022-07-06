// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;

namespace Line.Draw_Line_To_Clicked_Point_And_Generate_Shapes
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=pcLn2ze9JQA
    /// </summary>
    
    [RequireComponent(typeof(LineRenderer))]
    public class DrawLineToClickPointAndGenerateShapes_LineController : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private List<DrawLineToClickPointAndGenerateShapes_DotController> _dots = new List<DrawLineToClickPointAndGenerateShapes_DotController>();

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 0;
        }

        public void ToggleLoop()
        {
            _lineRenderer.loop = !_lineRenderer.loop;
        }

        public bool isLooped()
        {
            return _lineRenderer.loop;
        }
        private void LateUpdate()
        {
            if(_dots.Count < 2) return;
            
            for (int i = 0; i < _dots.Count; i++)
            {
                _lineRenderer.SetPosition(i,_dots[i].transform.position);
            }
        }

        public void SplitPointAtIndex(int index,
            out List<DrawLineToClickPointAndGenerateShapes_DotController> beforeDots,
            out List<DrawLineToClickPointAndGenerateShapes_DotController> afterDots)
        {
            List<DrawLineToClickPointAndGenerateShapes_DotController> before =
                new List<DrawLineToClickPointAndGenerateShapes_DotController>();

            List<DrawLineToClickPointAndGenerateShapes_DotController> after =
                new List<DrawLineToClickPointAndGenerateShapes_DotController>();

            int i = 0;

            for (; i < index; i++)
            {
                before.Add(_dots[i]);
            }

            i++;
            for (; i < _dots.Count; i++)
            {
                after.Add(_dots[i]);
            }

            beforeDots = before;
            afterDots = after;
        }
        
        public void AddPoint(DrawLineToClickPointAndGenerateShapes_DotController dot)
        {
            dot.index = _dots.Count;
            dot.SetLine(this);
            
            _lineRenderer.positionCount++;
            _dots.Add(dot);
        }
    }
}