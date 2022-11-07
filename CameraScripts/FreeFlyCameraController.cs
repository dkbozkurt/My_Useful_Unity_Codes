using System;
using Cinemachine;
using UnityEngine;

namespace CpiTemplate.Game.Creative.Controller
{
    public struct CameraShot
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 up;
        public float fieldOfView;
        public CameraShot(Vector3 position, Quaternion rotation, Vector3 up, float fieldOfView)
        {
            this.position = position;
            this.rotation = rotation;
            this.up = up;
            this.fieldOfView = fieldOfView;
        }
    }
    
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class FreeFlyCameraController : MonoBehaviour
    { 
        [SerializeField] private CinemachineVirtualCamera _fullFreeCamera;
        
        private CameraShot currentShot;
        
        [Header("Camera Settings")]
        public float movementSpeed = 20;
        public float rotationSpeed = 2;
        public float translationResponse = 10;
        public float rotationResponse = 10;
        public float fovResponse = 0;
        public float zoomMultiplier = 20f;
    
        private float ZoomMinBound = 0.1f;
        private float ZoomMaxBound = 179.9f;

        private bool _isCameraEnabled = false;
        private void Awake()
        {
            currentShot = new CameraShot(transform.position, transform.rotation, transform.up, _fullFreeCamera.m_Lens.FieldOfView);
        }
        private void LookAt(Vector3 position, Vector3 up)
            {
                currentShot.up = up;
                currentShot.rotation = Quaternion.LookRotation(position - currentShot.position, currentShot.up);
            }
    
        private void UpdateShot()
            {
                transform.position = Vector3.Lerp(transform.position, currentShot.position, translationResponse * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, currentShot.rotation, rotationResponse * Time.deltaTime);
                _fullFreeCamera.m_Lens.FieldOfView = Mathf.Lerp(_fullFreeCamera.m_Lens.FieldOfView, currentShot.fieldOfView, fovResponse * Time.deltaTime);
            }
    
        private void Zoom(float scroll)
        {
            _fullFreeCamera.m_Lens.FieldOfView += scroll * zoomMultiplier;
            _fullFreeCamera.m_Lens.FieldOfView = Mathf.Clamp(_fullFreeCamera.m_Lens.FieldOfView, ZoomMinBound, ZoomMaxBound);
        }
    
        private void Rotation()
        {
            float deltaX = Input.GetAxis("Mouse X") * rotationSpeed;
            float deltaY = Input.GetAxis("Mouse Y") * rotationSpeed;
            Quaternion fwd = currentShot.rotation * Quaternion.AngleAxis(deltaX, Vector3.up) * Quaternion.AngleAxis(deltaY, -Vector3.right);
            LookAt(currentShot.position + fwd * Vector3.forward, Vector3.up);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _isCameraEnabled = !_isCameraEnabled;
                _fullFreeCamera.enabled = _isCameraEnabled;
            }
        }

        private void LateUpdate()
        {
            Vector3 delta = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                delta += _fullFreeCamera.transform.forward;
            if (Input.GetKey(KeyCode.A))
                delta -= _fullFreeCamera.transform.right;
            if (Input.GetKey(KeyCode.S))
                delta -= _fullFreeCamera.transform.forward;
            if (Input.GetKey(KeyCode.D))
                delta += _fullFreeCamera.transform.right;
            if (Input.GetKey(KeyCode.Q))
                delta += _fullFreeCamera.transform.up;
            if (Input.GetKey(KeyCode.E))
                delta -= _fullFreeCamera.transform.up;
            
            
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                Zoom(-Input.GetAxis("Mouse ScrollWheel"));
            }

            if (Input.GetMouseButtonDown(2))
            {
                _fullFreeCamera.m_Lens.FieldOfView = 60f;
            }

            currentShot.position += delta * Time.deltaTime * movementSpeed;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Rotation();
            }

            UpdateShot();
        }
    
        //

        // private void Start()
        // {
        //     
        // }
        //
        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.F))
        //     {
        //         EnableFullFreeCameraController();
        //     }
        //
        //     if (Input.GetKeyDown(KeyCode.G))
        //     {
        //         EnableCreativeFlyCameraController();
        //     }
        // }
        //
        // private void EnableFullFreeCameraController()
        // {
        //     _creativeFlyCamera.enabled = false;
        //     _fullFreeCamera.enabled = true;
        //     
        //     
        // }
        //
        // private void EnableCreativeFlyCameraController()
        // {
        //     _fullFreeCamera.enabled = false;
        //     _creativeFlyCamera.enabled = true;
        //     
        //     
        // }
    }
}
