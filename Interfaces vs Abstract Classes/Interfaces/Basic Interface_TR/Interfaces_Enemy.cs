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

public class Interfaces_Enemy : MonoBehaviour, IDamageable<int>, IKillable
{

    public int Health { get; set; } = 100;
    
    public void Damage(int damageTaken)
    {
        Health -= damageTaken;
    }

    public void Kill()
    {
        Health = 0;
    }
    
    



}
