// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// How to save and load from PlayerPrefs example
///
/// PlayerPrefs.Get/Set(String,Int,Float)
/// 
/// </summary>

public class SetGetPlayerPrefs : MonoBehaviour
{
    private int score;
    private int highScore;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RollDice();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Reset();
        }
    }

    public void RollDice()
    {
        int number = UnityEngine.Random.Range(1, 7);
        score = number;

        if (number > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore",score);
            highScore = score;
        }
        
        Print();
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("HighScore");

        // Use the following with caution to delete all of the saved keys.
        // PlayerPrefs.DeleteAll();

        Debug.Log("HighScore set to 0 !");
    }

    private void Print()
    {
        Debug.Log("Score is: " + score + "High Score is: " + highScore);
    }
}
