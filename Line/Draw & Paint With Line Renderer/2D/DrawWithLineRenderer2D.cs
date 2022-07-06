// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace Line.Draw___Paint_With_Line_Renderer._2D
{
    /// <summary>
    /// Draw in Unity with Line Renderer in 2D space
    ///
    /// Attach this script onto empty GameObject and assign brushPrefab with "LineRenderer" component on it.
    ///
    /// NOTE : Camera Projection must be Orthographic.
    /// 
    /// Ref : https://www.youtube.com/watch?v=_ILOVprdq4o&ab_channel=1MinuteUnity
    /// </summary>

    public class DrawWithLineRenderer2D : MonoBehaviour
    {
        [SerializeField] private GameObject brushPrefab;
        private Camera _mainCam;

        private LineRenderer _currentLineRenderer;

        private Vector2 _lastPosition;

        private void Awake()
        {
            _mainCam = Camera.main;
            _mainCam.orthographic = true;
        }

        private void Update()
        {
            Draw();
        }

        private void Draw()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CreateBrush();
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                var mousePosition = GetMousePosition();
                if (mousePosition == _lastPosition) return;

                AddPoint(mousePosition);
                _lastPosition = mousePosition;
            }
            else _currentLineRenderer = null;
        }

        private Vector2 GetMousePosition()
        {
            return _mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        private void CreateBrush()
        {
            var brushInstance = Instantiate(brushPrefab);
            var mousePosition = GetMousePosition();
            _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

            _currentLineRenderer.SetPosition(0,mousePosition);
            _currentLineRenderer.SetPosition(1,mousePosition);
            
        }

        private void AddPoint(Vector2 pointPosition)
        {
            _currentLineRenderer.positionCount++;
            int positionIndex = _currentLineRenderer.positionCount - 1;
            _currentLineRenderer.SetPosition(positionIndex,pointPosition);
        }
        
    }
}
