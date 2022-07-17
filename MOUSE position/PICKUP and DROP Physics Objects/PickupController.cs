// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace MousePosition___MouseClick.PICKUP_and_DROP_Physics_Objects
{
    /// <summary>
    /// 
    /// NOTE: Attach this script onto the main camera which is a child of character controller
    /// scripts attached object's child object. Assign a hold area gameobject which is a child
    /// of main camera. 
    /// 
    /// Ref : https://www.youtube.com/watch?v=6bFCQqabfzo
    /// </summary>
    public class PickupController : MonoBehaviour
    {
        [Header("Pickup Settings")] [SerializeField]
        private Transform holdArea;

        private GameObject _heldObject;
        private Rigidbody _heldObjectRigidBody;

        [Header("Physics Parameters")] [SerializeField]
        private float pickupRange = 5.0f;

        [SerializeField] private float pickupForce = 150.0f;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_heldObject) // _heldObject == null
                {
                    CheckForPickableObject();
                }
                else
                {
                    DropObject();
                }
            }

            if (_heldObject) // _heldObject != null
            {
                MoveObject();
            }
        }

        private void CheckForPickableObject()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                    out hit, pickupRange))
            {
                PickupObject(hit.transform.gameObject);
            }
        }

        private void PickupObject(GameObject pickObj)
        {
            if (!pickObj.GetComponent<Rigidbody>()) return;

            _heldObjectRigidBody = pickObj.GetComponent<Rigidbody>();
            _heldObjectRigidBody.useGravity = false;
            _heldObjectRigidBody.drag = 10;
            _heldObjectRigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            _heldObjectRigidBody.transform.parent = holdArea;
            _heldObject = pickObj;
        }

        private void DropObject()
        {
            _heldObjectRigidBody.useGravity = true;
            _heldObjectRigidBody.drag = 1;
            _heldObjectRigidBody.constraints = RigidbodyConstraints.None;

            _heldObject.transform.parent = null;
            _heldObject = null;
        }

        private void MoveObject()
        {
            if (Vector3.Distance(_heldObject.transform.position, holdArea.position) > 0.1f)
            {
                Vector3 moveDirection = (holdArea.position - _heldObject.transform.position);
                _heldObjectRigidBody.AddForce(moveDirection * pickupForce);
            }
        }
        
        
    }
}