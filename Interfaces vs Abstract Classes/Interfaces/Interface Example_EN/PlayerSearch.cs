// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=2LA3BLqOw9g
/// 
/// </summary>

public class PlayerSearch : MonoBehaviour
{
    private void Update()
    {
        var nearestGameObject = GetNearestGameObject();
        if (nearestGameObject == null) return;

        if (Input.GetButtonDown("Fire1"))
        {
            var interactable = nearestGameObject.GetComponent<IInteractable>();
            if (interactable == null) ;
            interactable.Interact();
        }
    }

    private GameObject GetNearestGameObject()
    {
        return Instantiate(gameObject, Vector3.one, Quaternion.identity); 
    }
}
