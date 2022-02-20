// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private ActionOnTimer actionOnTimer;

    private void Start()
    {
        actionOnTimer.SetTimer(1f, () => { Debug.Log("Timer completed!"); });
    }
}
