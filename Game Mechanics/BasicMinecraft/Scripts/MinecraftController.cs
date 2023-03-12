// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;

namespace Game_Mechanics.BasicMinecraft.Scripts
{
    /// <summary>
    /// Ref : https://www.youtube.com/watch?v=fUlrBNZkl9w
    /// </summary>

    public class MinecraftController : MonoBehaviour
    {
        [SerializeField] private Vector2 _cameraSpeed = new Vector2(180, 180);
        [SerializeField] private float _moveSpeed = 10f;

        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _blockOutline;

        private string _blockPrefabName;

        private float _pitch = 0;
        private float _yaw = 0;
        
        private Camera _mainCamera;

        private RaycastHit _hit;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;

            _blockPrefabName = GetPrefabName();
        }

        private void Update()
        {
            CameraMovement();

            Detection();

        }
        
        private void CameraMovement()
        {
            // look around
            _pitch += -Input.GetAxis("Mouse Y") * _cameraSpeed.x * Time.deltaTime;
            _yaw += Input.GetAxis("Mouse X") * _cameraSpeed.y * Time.deltaTime;

            _mainCamera.transform.eulerAngles = new Vector3(_pitch, _yaw, 0);
            
            // get input values
            float inputX = Input.GetAxis("Horizontal");
            float inputY = 0f;
            float inputZ = Input.GetAxis("Vertical");
            
            // directions
            Vector3 dirForward = Vector3.ProjectOnPlane(_mainCamera.transform.forward, Vector3.up).normalized;
            Vector3 dirSide = _mainCamera.transform.right;
            Vector3 dirUp = Vector3.up;

            Vector3 moveDir = (inputX * dirSide) + (inputY * dirUp) + (inputZ * dirForward);

            _mainCamera.transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }

        private void Detection()
        {
            Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            
            // Looking at a block
            if (Physics.Raycast(ray, out _hit))
            {
                Vector3 pos = _hit.point;

                // Move away from the surface slightly
                pos += _hit.normal * 0.1f;
                
                // Round the position values to whole numbers
                pos = new Vector3(
                    Mathf.Floor(pos.x),
                    Mathf.Floor(pos.y),
                    Mathf.Floor(pos.z));

                // offset position by half a block
                pos += Vector3.one * 0.5f;
                
                // Set Outline block position for preview
                _blockOutline.transform.position = pos;
                
                // Left Click to generate
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(_blockPrefab, pos, Quaternion.identity);
                }
                
                // Right Click to remove
                else if (Input.GetMouseButtonDown(1))
                {
                    if (_hit.collider.name == _blockPrefabName)
                    {
                        Destroy(_hit.collider.gameObject);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            // Camera Ray
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_mainCamera.transform.position,_mainCamera.transform.forward * 99999);
            
            // Collision Point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_hit.point,0.05f);
            
            // Surface direction
            Gizmos.DrawRay(_hit.point, _hit.normal);
        }
        
        private string GetPrefabName()
        {
            return _blockPrefab.name + "(Clone)";
        }
    }
}
