// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=G1bd75R10m4
/// </summary>
public class BattleSystem : StateMachine
{
    #region Fields and Properties
    

    #endregion

    #region Execution

    private void Start()
    {
        SetState(new Begin(this));
    }

    public void OnAttackButton()
    {
        StartCoroutine(State.Attack());
    }

    public void OnHealButton()
    {
        StartCoroutine(State.Heal());
    }

    #endregion
    
}
