// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// 
/// Free fly camera to look around and zoom in/out.
/// Attach this script to camera.
/// 
/// Mouse Right Click : To rotate
/// W A S D : To Move around
/// Mouse Scroll: Zoom in/out 
/// 
/// </summary>
[RequireComponent(typeof(Camera))]
public class FreeFlyCamera : MonoBehaviour
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
    private Camera cam;
    private CameraShot currentShot;
    
    [Header("Camera Settings")]
    public float movementSpeed = 5;
    public float rotationSpeed = 8;
    public float translationResponse = 10;
    public float rotationResponse = 10;
    public float fovResponse = 0;
    public float zoomMultiplier = 20f;

    private float ZoomMinBound = 0.1f;
    private float ZoomMaxBound = 179.9f;
    private void Awake()
       {
           cam = GetComponent<Camera>();
           currentShot = new CameraShot(transform.position, transform.rotation, transform.up, cam.fieldOfView);
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
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentShot.fieldOfView, fovResponse * Time.deltaTime);
        }

    private void Zoom(float scroll)
    {
        cam.fieldOfView += scroll * zoomMultiplier;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, ZoomMinBound, ZoomMaxBound);
    }

    private void Rotation()
    {
        float deltaX = Input.GetAxis("Mouse X") * rotationSpeed;
        float deltaY = Input.GetAxis("Mouse Y") * rotationSpeed;
        Quaternion fwd = currentShot.rotation * Quaternion.AngleAxis(deltaX, Vector3.up) * Quaternion.AngleAxis(deltaY, -Vector3.right);
        LookAt(currentShot.position + fwd * Vector3.forward, Vector3.up);
    }
    
    private void LateUpdate()
        {
            Vector3 delta = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                delta += cam.transform.forward;
            if (Input.GetKey(KeyCode.A))
                delta -= cam.transform.right;
            if (Input.GetKey(KeyCode.S))
                delta -= cam.transform.forward;
            if (Input.GetKey(KeyCode.D))
                delta += cam.transform.right;
            if (Input.GetKey(KeyCode.E))
                delta += cam.transform.up;
            if (Input.GetKey(KeyCode.Q))
                delta -= cam.transform.up;
            
            
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                Zoom(-Input.GetAxis("Mouse ScrollWheel"));
            }

            if (Input.GetMouseButtonDown(2))
            {
                cam.fieldOfView = 60f;
            }

            currentShot.position += delta * Time.deltaTime * movementSpeed;

            if (Input.GetKey(KeyCode.Mouse1))
            {
                Rotation();
            }

            UpdateShot();
        }
}
