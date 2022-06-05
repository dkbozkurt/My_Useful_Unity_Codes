// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// We cant see "Serializable_DamageValues" variable in the inspector, because it is not a native class
/// to unity isn't serialized by default.
///
/// Ref : https://www.youtube.com/watch?v=5fhTXnos_go
/// </summary>

public class Serializable_Weapon : MonoBehaviour
{
    
    [SerializeField] private string weaponID;
    public string weaponName;
    public int cost;
    public Serializable_DamageValues damageValues;



}
