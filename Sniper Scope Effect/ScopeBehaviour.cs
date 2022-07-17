// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using UnityEngine;

namespace Sniper_Scope_Effect
{
    /// <summary>
    ///
    /// Note: Do not forget to add a new child camera of the main camera ta render only
    /// weapon's layer. And set the weapon's camera "Depth" to 1.
    /// 
    /// Ref : https://www.youtube.com/watch?v=adcKX1c-kag
    /// </summary>
    
    public class ScopeBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator weaponAnimator;
        [SerializeField] private float animationTransitionDuration;

        [SerializeField] private GameObject scopeOverlay;
        [SerializeField] private GameObject weaponCamera;
        
        [SerializeField] private float scopedFOV = 15f;
        private float _normalFOV;

        private Camera _mainCamera;
        private bool _isScoped = false;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _normalFOV = _mainCamera.fieldOfView;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                ScopeEffect();    
            }
        }

        private void ScopeEffect()
        {
            _isScoped = !_isScoped;
            weaponAnimator.SetBool("Scoped",_isScoped);

            if (_isScoped)
                OnScoped();
            else
                OnUnscoped();
        }

        private void OnUnscoped()
        {
            scopeOverlay.SetActive(false);
            weaponCamera.SetActive(true);
            
            _mainCamera.fieldOfView = _normalFOV;
        }

        private void OnScoped()
        {
            StartCoroutine(Do());

            IEnumerator Do()
            {
                yield return new WaitForSeconds(animationTransitionDuration);
                
                scopeOverlay.SetActive(transform);
                weaponCamera.SetActive(false);

                _mainCamera.fieldOfView = scopedFOV;

            }
            
        }
    }
}
