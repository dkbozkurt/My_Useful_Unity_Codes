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

public class ExampleIIScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int _score;
    
    private void OnEnable()
    {
        ExampleIIActions.OnEnemyKilled += UpdateScore;
    }

    private void OnDisable()
    {
        ExampleIIActions.OnEnemyKilled -= UpdateScore;
    }
    
    public void UpdateScore(ExampleIIEnemy enemyRef)
    {
        _score += enemyRef.scoreWhenKilled;
        scoreText.text = _score.ToString();
    }




}
