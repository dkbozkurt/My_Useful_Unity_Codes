// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attach this script to parent object of the player.
/// 
/// Note: Dont forget to decrease near "clipping planes" value from the camera component. 
/// </summary>

public class RaycastShooting : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GameObject shootingSource;
    [CanBeNull] [SerializeField] private Image crosshair;
    [CanBeNull] [SerializeField] private ParticleSystem shootingParticle;
    [CanBeNull] [SerializeField] private GameObject impactEffect;
    
    
    [Header("Variables")] 
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float impactForce = 30f;
    [SerializeField] private bool isCrosshairOn = true;
    
    private bool isHit = false;
    private float nextTimeToFire = 0f;
    

    private void Awake()
    {
        Crosshair(isCrosshairOn);
            
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();    
        }
    }

    private void Shoot()
    {
        if(shootingParticle != null) shootingParticle.Play();
        
        RaycastHit hit;
        // out hit: unity puts all gathered informations we neeed into hit variable.
        isHit = Physics.Raycast(shootingSource.transform.position, shootingSource.transform.forward, out hit, range);

        if (isHit)
        {
            // Debug.DrawLine(shootingSource.transform.position, hit.point);
            Damage();
        }
        
        Impact(hit);
    }

    private void Crosshair(bool status)
    {
        if(crosshair != null) crosshair.enabled = status;
    }
    

    private void Damage()
    {
        // Target target = isHit.transform.GetComponent<Target>();
        // if (target != null)
        // {
        //     target.TakeDamage(damage);
        // }
    }

    private void Impact(RaycastHit hit)
    {
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * impactForce );
        }

        if (hit.transform != null)
        {
            GameObject impactGameobject = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameobject, 2f);
        }
            
    }
}
