// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Add this script into the object that you wanna get trail effects behind of it.
///
/// Make an animation for the echo prefabs so it will looks cooler.
///
/// Ref : https://www.youtube.com/watch?v=_TcEfIXpmRI
/// </summary>

public class EchoTrailEffect : MonoBehaviour
{
    private float timeBetweenSpawn;
    [SerializeField] private float startTimeBetweenSpawns = 0.05f;
    [SerializeField] private float destroyEchoAfterSecs = 1f;
    [SerializeField] private GameObject[] echoPrefabs;

    private void Update()
    {
        EchoEffect();
    }

    private void EchoEffect()
    {
        //if (gameObject.GetComponent<Rigidbody>().velocity.magnitude != 0f)
        {
            if (timeBetweenSpawn <= 0)
            {
                // spawn echo game objects
                int rand = Random.Range(0, echoPrefabs.Length);
                GameObject instance = (GameObject) Instantiate(echoPrefabs[rand], transform.position, Quaternion.identity);
                Destroy(instance,destroyEchoAfterSecs);
                timeBetweenSpawn = startTimeBetweenSpawns;
            }
            else
            {
                timeBetweenSpawn -= Time.deltaTime;
            }
        }
        
    }
}
