//Dogukan Kaan Bozkurt
//      github.com/dkbozkurt
//GEFEASOFT

/*
 * Drag and drop the object on which the mouse is pressed and located. 
 * Be sure that object has collider
 * Add this script to the object's inspector.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mzCoord;

    //22,24-27 and 32. lines for moving child object out of it's parent.
    private Transform child;

    private void Awake()
    {
        //Assigned, script's attached object's transform into child.
        child = this.gameObject.transform;
    }
    private void OnMouseDown()
    {
        mzCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        //Moves the child object out of its parent object, if child object is selected.
        child.SetParent(null, true);    
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mzCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;
    }
}
