// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using Game.Scripts.Behaviours;
using UnityEngine;

namespace Game.Scripts.Managers
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=2IhzPTS4av4&ab_channel=CodeMonkey
    /// </summary>
    public class PickUpDropManager : SingletonBehaviour<PickUpDropManager>
    {
        [Header("Player Related")] 
        [SerializeField] private Transform objectGrabPointTransform;
        
        [Space]
        [Header("Grabbable Related")]
        [SerializeField] private LayerMask pickUpLayerMask;
        private GrabbableObject _grabbableObject;
        private float _pickUpDistance = 100f;
        
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(_grabbableObject) return;
                PickUpObject();
            }

            if (Input.GetMouseButtonUp(0))
            {
                if(!_grabbableObject) return;
                DropObject();
            }

        }

        private void PickUpObject()
        {
            // With Camera transform forward direction
            // if (Physics.Raycast(_mainCamera.transform,_mainCamera.forward , out RaycastHit raycastHit,float.MaxValue, pickUpLayerMask))
            
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit,float.MaxValue, pickUpLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out _grabbableObject))
                {
                    _grabbableObject.Grab(objectGrabPointTransform);
                }
            }
        }

        private void DropObject()
        {
            _grabbableObject.Drop();
            _grabbableObject = null;
        }
    }
}
