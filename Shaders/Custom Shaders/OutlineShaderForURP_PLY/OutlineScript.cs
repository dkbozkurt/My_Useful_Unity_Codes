// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

namespace OutlineShaderForURP__PLY
{
    /// <summary>
    /// 
    /// </summary>
    public class OutlineScript : MonoBehaviour
    {
        [SerializeField] private Material _outlineMaterial;
        [Range(1f,1.5f)]
        [SerializeField] private float _outlineScaleFactor = 1.1f;
        [SerializeField] private Color _outlineColor;
        private Renderer _outlineRenderer;

        void Start()
        {
            _outlineRenderer = CreateOutline(_outlineMaterial, _outlineScaleFactor, _outlineColor);
            _outlineRenderer.enabled = true;
        }
        Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
        {
            var realScaleFactor = scaleFactor * -1f;
            GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
            Renderer rend = outlineObject.GetComponent<Renderer>();

            rend.material = outlineMat;
            rend.material.SetColor("_OutlineColor", color);
            rend.material.SetFloat("_Scale", realScaleFactor);
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            outlineObject.GetComponent<OutlineScript>().enabled = false;
            outlineObject.GetComponent<Collider>().enabled = false;

            rend.enabled = false;

            return rend;
        }
    }
}