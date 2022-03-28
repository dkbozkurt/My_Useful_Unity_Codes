// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Held the Actions so it is important !!! 

/// <summary>
/// Actions in Unity
/// 
/// Note : If your having any null ref exception, it ll be most probably because
/// there is no listeners of the Action in the scene.
/// 
/// Ref: https://www.youtube.com/watch?v=8fcI8W9NBEo
/// </summary>
public class ExampleIIEnemy : MonoBehaviour
{
    public int scoreWhenKilled;
    private float _speed = 3;

    private void Update()
    {
        transform.Translate(Vector3.right*_speed*Time.deltaTime);
    }

    private void OnMouseDown()
    {
        // First way to check if the action has any listeners
        // if (ExampleIIActions.OnEnemyKilled != null)
        //     ExampleIIActions.OnEnemyKilled(this);
        
        // Second way to check if the action has any listeners
        ExampleIIActions.OnEnemyKilled?.Invoke(this);
        
        Destroy(gameObject);
        
    }
}
