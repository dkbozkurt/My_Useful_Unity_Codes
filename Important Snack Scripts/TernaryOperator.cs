// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ternary Operator is basically bool operation in a single line.
/// </summary>

public class TernaryOperator : MonoBehaviour
{
    // Assigning variables with a single line of bool check.
    private string _message="";
    private int _health;
    
    private void Start()
    {
        //TernaryOp();
        IfStatement();
    }

    private void TernaryOp()
    {
        _health = 100;

        _message = _health > 0 ? "Player is Alive." : "Player is Dead.";

        Debug.Log($"Message {_message}");
        
        //
        
        _health = 0;

        _message = _health > 0 ? "Player is Alive." : _health == 0 ? "Player is Barely Alive." : "Player is Dead.";
        
        Debug.Log($"Message {_message}");

    }
    private void IfStatement()
    {
        _health = 100;
        
        if (_health > 0)
            _message = "Player is Alive.";
        else
            _message = "Player is Dead.";

        Debug.Log($"Message {_message}");
        
        //
        
        _health = 0;
        
        if(_health > 0)
            _message = "Player is Alive.";
        else
        {
            if (_health == 0)
                _message = "Player is Barely Alive.";
            else
                _message = "Player is Dead.";
        }
        
        Debug.Log($"Message {_message}");

    }
    



}
