// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Ref : https://www.youtube.com/watch?v=YCHJwnmUGDk
/// </summary>

public class ObjectPoolIntroBullet : MonoBehaviour
{
    #region Other Methods

    

    #endregion

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
}
