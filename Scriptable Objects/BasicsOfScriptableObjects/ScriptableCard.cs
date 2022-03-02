// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Scriptable Objects - 
/// * Smart data containers that can hold values that also exist outside of Play Mode 
/// * Globally accessible and scene-independent.
/// 
/// Derive from ScriptableObject instead of MonoBehaviour.
///
/// This script is only the template for our objects. To create a object from this template
/// we must add the attribute called"[CreateAssetMenu]" at the beginning of the class.
/// This will basically tell unity to make it possible to create this object through the create asset menu.
///
/// Ref : https://www.youtube.com/watch?v=aPXvoWVabPY&ab_channel=Brackeys
/// </summary>

// After than adding this attribute, now we can create a new file called "New Scriptable Card" in
// the project window by using Create>Card>Example1
[CreateAssetMenu(fileName = "New Scriptable Card",menuName = "Card/Example1")] // Important !!!
public class ScriptableCard : ScriptableObject
{
    //[SerializeField] private string name;
    // By default any object already has a variable called "name" so we can either
    // rename this to say "cardName" or we can just tell the object to use this
    // definition of name from here on out by using public "new" string...
    public new string name;
    public string description;

    public Sprite artwork;

    public int manaCost;
    public int attack;
    public int health;

    public void Print()
    {
        Debug.Log(name + ": " + description + " The card costs: " + manaCost);
    }
    
    // Working queue of the main methods of scriptable objects

    // Calls when the scriptable object is created, not at the beginning of the game !!!
    private void Awake() { }
    
    // Calls first at the game beginning of the game.
    private void OnDisable() { }

    // Calls secondly at the game beginning of the game. Called when a script is loaded or the value of the script changes in the inspector.
    private void OnValidate() { }

    // Calls thirdly at the game beginning of the game. Also calls after than awake method, when a scriptable object is created.
    private void OnEnable() { }

    private void OnDestroy() { }
}
