// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace RayCast
{
    /// <summary>
    /// Selecting Objects with Raycast
    /// 
    /// Ref : https://www.youtube.com/watch?v=_yf5vzZ2sYE&t=4s
    /// </summary>

    public class ObjectSelectingWithRaycast : MonoBehaviour
    {
        [SerializeField] private Material highlightMaterial;
        [SerializeField] private Material defaultMaterial;

        [SerializeField] private LayerMask selectableObjectsLayerMask;
        private Transform _selection;

        private void Update()
        {
            if (_selection = null)
            {
                var selectionRenderer = _selection.GetComponent<Renderer>();
                selectionRenderer.material = defaultMaterial;
                _selection = null;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Single.PositiveInfinity, selectableObjectsLayerMask))
            {
                var selection = hit.transform;

                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }

                _selection = selection;
            }
        }

    }
}