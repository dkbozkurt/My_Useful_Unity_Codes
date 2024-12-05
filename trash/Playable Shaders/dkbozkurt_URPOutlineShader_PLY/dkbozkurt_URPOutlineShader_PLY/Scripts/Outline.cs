// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace URPOutlineShader_PLY.Scripts
{
    /// <summary>
    ///  
    /// </summary>
    public class Outline : MonoBehaviour
    {
        [SerializeField] private Material _outlineMaterial;
        [SerializeField] private float _outlineScaleFactor = 1.1f;
        [SerializeField] private Color _outlineColor = Color.black;

        [Space] 
        [SerializeField] private bool _isScaleUpdatable = false;
        [SerializeField] private float _outlineScaleLive = 1.1f;

        private int _inverterValue = -1;
        private Renderer _outlineRenderer;

        void Start()
        {
            _outlineRenderer = CreateOutline(_outlineMaterial, _outlineScaleFactor, _outlineColor);
            _outlineRenderer.enabled = true;
        }
        
        Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
        {
            var invertedValue = _inverterValue * scaleFactor;
            GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
            Renderer rend = outlineObject.GetComponent<Renderer>();

            rend.material = outlineMat;
            rend.material.SetColor("_OutlineColor", color);
            rend.material.SetFloat("_Scale", invertedValue);
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            outlineObject.GetComponent<Outline>().enabled = false;
            outlineObject.GetComponent<Collider>().enabled = false;

            rend.enabled = false;

            return rend;
        }

        private void Update()
        {
            if(!_isScaleUpdatable) return;

            SetOutlineScale(_outlineScaleLive);
        }

        public void OutlineSetter(bool status)
        {
            if (!status)
            {
                SetOutlineScale(1);
                return;
            }
            
            SetOutlineScale(2);
        }

        private void SetOutlineScale(float finalValue)
        {
            _outlineRenderer.material.SetFloat("_Scale", _inverterValue * finalValue);
        }
        
        
    }
}