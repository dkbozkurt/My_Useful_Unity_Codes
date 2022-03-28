// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// C# Actions
///
/// Basic Action working concept
/// 
/// Ref: https://www.youtube.com/watch?v=fmGr2OxrM1Q&ab_channel=UnityClassroom
/// </summary>

public class ActionPlayer : MonoBehaviour
{
    public static Action<int> OnDamageReceived;
    public int Health { get; set; }
    
    private void Start()
    {
        Health = 10;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Damage();
        }
    }

    private void Damage()
    {
        Health--;
        if (OnDamageReceived != null) OnDamageReceived(Health);

    }
}
