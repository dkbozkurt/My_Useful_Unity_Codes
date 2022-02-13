// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

/// <summary>
/// Attach this script to parent object of the player.
/// 
/// </summary>

public class GameObjectShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosition;
    
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float shootDelay = 0.1f;
    [SerializeField] private float destoryBullettime = 2f;

    private bool isShooting = false;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (!isShooting)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            Bullet(bullet);
            
            isShooting = true;
            StartCoroutine(WaitForDelay(shootDelay));
        }
        
    }

    private void Bullet(GameObject bullet)
    {
        bullet.transform.position = bulletSpawnPosition.position;
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPosition.forward * bulletSpeed;
        Destroy(bullet,destoryBullettime);
    }

    private IEnumerator WaitForDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isShooting = false;
    }

}
