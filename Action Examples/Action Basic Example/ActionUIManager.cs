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

public class ActionUIManager : MonoBehaviour
{
    public void OnEnable()
    {
        ActionPlayer.OnDamageReceived += UpdateHealth;
    }

    public void OnDisable()
    {
        ActionPlayer.OnDamageReceived -= UpdateHealth;
    }

    private void UpdateHealth(int health)
    {
        Debug.Log("Current Health : " + health);
    }
}
