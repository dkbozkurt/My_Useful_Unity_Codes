// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnTimer : MonoBehaviour
{
    private Action timerCallBack;
    private float timer;

    public void SetTimer(float timer, Action timerCallBack)
    {
        this.timer = timer;
        this.timerCallBack = timerCallBack;
    }

    private void Update()
    {
        if (timer >= 0f)
        {
            timer -= Time.deltaTime;

            if (IsTimerComplete())
            {
                timerCallBack();
            }
        }
    }

    public bool IsTimerComplete()
    {
        return timer <= 0f;
    }

}
