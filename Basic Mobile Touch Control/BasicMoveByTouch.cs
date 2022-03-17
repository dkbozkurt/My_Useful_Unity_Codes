// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script onto an empty gameobject that will control interactions with touches.
///
/// Use Joystick Pack with this script.
///  
/// NOTES:
/// Input.touchCount, checks how many fingers are touching onto screen.
/// Input.GetTouch(i) i'th touch information.
/// touch.phase gives us such information (Began, Ended, Moved, Stationary, Canceled)
/// Input.touches, hold all of the touch information in an array.
///
/// Ref : https://www.youtube.com/watch?v=bp2PiFC9sSs&ab_channel=Brackeys
/// </summary>
public class BasicMoveByTouch : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Update()
    {
        //MoveCharacterToTouchPosition();
        
        //DrawLineToAllTouches();
        
    }

    private void MoveCharacterToTouchPosition()
    {
        // Are there any touches
        if (Input.touchCount > 0)
        {
            // Input.GetTouch(i) i'th touch information.
            Touch touch = Input.GetTouch(0);
            
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;
            player.transform.position = touchPosition;

        }
    }

    private void DrawLineToAllTouches()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            Debug.DrawLine(Vector3.zero, touchPosition, Color.red);
        }
    }
}