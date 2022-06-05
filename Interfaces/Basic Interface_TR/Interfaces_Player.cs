// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tek bir üst sınıf alabildiği ve bunu MonoBehaviour'dan kullandiği için, interfaceden implement ettik ve interfaceye ait
/// fonksiyonları classimizda olusturduk.
///
/// 
/// Ref :https://www.youtube.com/watch?v=nFcYxYdBgrw
/// </summary>

public class Interfaces_Player : MonoBehaviour, IDamageable<float>, IKillable
{
    private float _health = 100;

    public float Health
    {
        get => _health;
        set { _health = value; }
    }
     
    public void Damage(float damageTaken)
    {
        _health -= damageTaken;
    }

    public void Kill()
    {
        _health = 0;
    }

}