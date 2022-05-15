// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

/// <summary>
///
/// Ref : https://www.youtube.com/watch?v=YCHJwnmUGDk
/// </summary>

public class ObjectPoolIntroTest : MonoBehaviour
{
    #region Other Methods

    

    #endregion

    [SerializeField]private Transform bulletPosition;
    private void Fire()
    {
        GameObject bullet = ObjectPoolIntro.instance.GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = bulletPosition.position;
            bullet.SetActive(true);
        }
    }
}
