// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI; // Held the Actions so it is important !!! 

/// <summary>
/// Actions in Unity
/// 
/// Ref: https://www.youtube.com/watch?v=8fcI8W9NBEo
/// </summary>
public class ExampleIIStatsManager : MonoBehaviour
{
    public Text killsText;
    private int _killCount;

    private void OnEnable()
    {
        ExampleIIActions.OnEnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        ExampleIIActions.OnEnemyKilled -= EnemyKilled;
    }

    public void EnemyKilled(ExampleIIEnemy enemyRef)
    {
        _killCount++;
        killsText.text = _killCount.ToString();
    }
}
