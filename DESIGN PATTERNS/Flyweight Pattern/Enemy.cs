// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt


using System;
using UnityEngine;

/// <summary>
/// Flyweight Design Pattern
/// 
/// Ref : https://www.youtube.com/watch?v=fwgkEpxUifQ&t=58s
/// </summary>

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData = null;

    private float _currentSpeed = 10f;
    private int _currentHp = 100;

    private void Start()
    {
        var speed = enemyData.MaxSpeed;
    }
}
