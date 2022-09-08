// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;
using UnityEngine.UI;

namespace _3DMovement
{
    /// <summary>
    /// Smooth Move Rotate and Float change operations by using Lerp
    /// 
    /// Ref : https://www.youtube.com/watch?v=jAN2IoWdPzM&ab_channel=CodeMonkey
    /// </summary>
    public class SmoothMoveRotateFloatWithLerp : MonoBehaviour
    {
        [Header("Lerp Speed")]
        [SerializeField] private float lerpSpeed;
        
            
        [Header("Target values")] 
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Image image;
        [SerializeField] private float targetFillAmount;
        
        private void Update()
        {
            // SmoothMove();
            
            // SmoothRotate();
            
            // SmoothFloat();
        }

        private void SmoothMove()
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position,lerpSpeed * Time.deltaTime);
        }

        private void SmoothRotate()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation,lerpSpeed * Time.deltaTime);
        }

        private void SmoothFloat()
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, targetFillAmount, Time.deltaTime * lerpSpeed);
        }
    }
}
