// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Abstract Class
///
/// Abstract classes usually implement some shared behaviour.
///
/// Abstract class object can be used with SerializeField and public means can assignable from inspector.
///
/// "FindObjectsOfTypes"
/// 
/// Ref : https://www.youtube.com/watch?v=xI4QWXS6vAU
/// </summary>

public abstract class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int _health;

    private void Awake()
    {
        _health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if(_health <= 0) Destroy(gameObject);
    }
}
