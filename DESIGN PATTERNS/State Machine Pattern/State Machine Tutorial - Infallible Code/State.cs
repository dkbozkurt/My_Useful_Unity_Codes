// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State Machine Pattern mostly uses in animation switching and turn based games.
///
/// Ref : https://www.youtube.com/watch?v=G1bd75R10m4
/// </summary>

// We add 3 methods that each state have to implement (Start, Attack, Heal). Each one has to be virtual so it can be overriden by the driven class.
// By default we left yield break, that way we dont have to ga and implement every class, if they are not overriden, then will just go ahead and
// yield break by default.

public abstract class State
{
    protected BattleSystem BattleSystem;
    
    // Constructor
    public State(BattleSystem battleSystem)
    {
        BattleSystem = battleSystem;
    }
    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator Attack()
    {
        yield break;    
    }

    public virtual IEnumerator Heal()
    {
        yield break;
    }
    
}
