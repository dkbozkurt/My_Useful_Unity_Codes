// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using TMPro;
using UnityEngine;

namespace BasicMouseDisplacement
{
    /// <summary>
    /// 
    /// </summary>

    public class CatchMouse : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _initialText;
        [SerializeField] private TextMeshProUGUI _currentText;
        [SerializeField] private TextMeshProUGUI _displacementText;
        
        private float _initialX = 0f;

        private void Update()
        {
            var pointX = ScreenCenteredPosition().x;
            
            if (Input.GetMouseButtonDown(0))
            {
                Initial(pointX);
            }
            if (Input.GetMouseButton(0))
            {
                Current(pointX);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Displacement(pointX - _initialX);
            }
        }

        private Vector2 ScreenCenteredPosition()
        {
            var screenPosition = Input.mousePosition;
            var x = Screen.width;
            var y = Screen.height;
            return screenPosition - new Vector3(x / 2,y/2, 0);
        }

        private void Current(float x)
        {
            _currentText.text = "CurrentX : " + x.ToString();
        }
    
        private void Displacement(float x)
        {
            _displacementText.text = "DisplacementX : " + x.ToString();
        }
    
        private void Initial(float x)
        {
            _initialX = x;
            _initialText.text = "InitialX : " + x.ToString();
        }
    }
}
