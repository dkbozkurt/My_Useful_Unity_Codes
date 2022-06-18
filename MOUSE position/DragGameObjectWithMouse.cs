// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Drop this script onto a gameObject to be moved.
/// 
/// Ref : https://answers.unity.com/questions/12322/drag-gameobject-with-mouse.html
/// </summary>

[RequireComponent(typeof(Collider))]
public class DragGameObjectWithMouse : MonoBehaviour
{
    private Vector3 _distanceToScreen;
    private Vector3 _offset;

    void OnMouseDown()
    {
        _distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position);
 
        _offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distanceToScreen.z));
 
    }

    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distanceToScreen.z);
        
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
        transform.position = curPosition;
    }
}
