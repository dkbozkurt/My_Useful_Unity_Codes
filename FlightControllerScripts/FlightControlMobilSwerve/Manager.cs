// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Type 3

/// <summary>
/// Add this script to empty gameobject to use with "PlayerMotor" script to control an object in 3D space.
/// </summary>
public class Manager : MonoBehaviour
{
    public static Manager Instance { set; get; }
    private Dictionary<int, Vector2> activeTouches = new Dictionary<int, Vector2>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    
    // Read all touches from user
    public Vector3 GetPlayerInput()
    {
        Vector3 r = Vector3.zero;

        foreach (Touch touch in Input.touches)
        {
            // If we just started pressing on the screen
            if (touch.phase == TouchPhase.Began)
            {
                activeTouches.Add(touch.fingerId,touch.position);
            }
            
            // If we remove our finger off the screen
            else if (touch.phase == TouchPhase.Ended)
            {
                if (activeTouches.ContainsKey(touch.fingerId))
                    activeTouches.Remove(touch.fingerId);
            }
            
            // Out finger is either moving, or stationary, in both cases, let's use the delta
            else
            {
                float mag = 0;
                r = (touch.position - activeTouches[touch.fingerId]);
                mag = r.magnitude / 300;
                r = r.normalized * mag;
            }
        }
        return r;
    }
}
