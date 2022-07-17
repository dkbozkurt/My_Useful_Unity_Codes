// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using UnityEngine;

namespace Slow_Motion
{
    /// <summary>
    /// Create Explosion at the pointed location.
    ///
    /// Use with "SlowTimeManager.cs"
    /// 
    /// Ref : https://www.youtube.com/watch?v=0VGosgaoTsw
    /// </summary>
    
    public class ExplosionGun : MonoBehaviour
    {
        [SerializeField] private GameObject explosion;
        private Camera _camera;

        [SerializeField] private SlowTimeManager slowTimeManager;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hitInfo))
            {
                Instantiate(explosion, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                
                slowTimeManager.DoSlowMotion();
            }
        }
    }
}