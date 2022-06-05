// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// We make this class "Serializable" so when other classes are trying to use variable
/// properties of this class can show the variables in the inspector. 
/// Ref : https://www.youtube.com/watch?v=5fhTXnos_go
/// </summary>

[System.Serializable]
public class Serializable_DamageValues
{

    public int physicalDamage;
    public int fireDamage;
    public int shockDamage;

}
