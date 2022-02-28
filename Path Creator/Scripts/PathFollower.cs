// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using UnityEngine;
using PathCreation; // Dont forget to import !

/// <summary>
/// Attach this script to the gameObject that will follow the path.
///
/// Add "Path Creator.cs" script into an empty object, that will hold the path information.
/// 
/// NOTE: Use this script with " Bézier Path Creator " tools from asset store.
///
/// Ref : https://www.youtube.com/watch?v=saAQNRSYU9k
/// </summary>

public class PathFollower : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private float speed = 5f;
    private float distanceTravelled;
    [SerializeField] private bool enableObjectRotation = true;

    private void Update()
    {
        Follower();
    }

    private void Follower()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        if (enableObjectRotation)
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

    }
}
