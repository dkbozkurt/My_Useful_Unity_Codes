// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
///
/// Ref : https://www.youtube.com/watch?v=xI4QWXS6vAU
/// </summary>

public class Abstract_DamageOnClick : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Must for interfaces
            //
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // Interface example
                // ITakeDamage damageable =  hitInfo.collider.GetComponent<ITakeDamage>(); 
                
                // Abstract class example
                Damageable damagable = hitInfo.collider.GetComponent<Damageable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(1);
                }
            }
            //
            
            // Can be used with Abstract Classes.
            //
            var damageables = FindObjectsOfType<Damageable>();

            foreach (var damageable in damageables)
            {
                damageable.TakeDamage(1);
            }
            //

        }
    }
}
