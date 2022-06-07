// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inheritance
/// -Base Class-
/// 
/// Ref : https://www.youtube.com/watch?v=8TIkManpEu4
/// </summary>

public class Inheritance_Character : MonoBehaviour
{

    [SerializeField] protected string firstName;

    [SerializeField] protected float moveSpeed = 1f;

    [SerializeField] protected int startingHealth = 100;

    private int _health;

    protected virtual void Update()
    {
        Debug.Log("Base classes Update method.");
    }

    private void Awake()
    {
        _health = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
