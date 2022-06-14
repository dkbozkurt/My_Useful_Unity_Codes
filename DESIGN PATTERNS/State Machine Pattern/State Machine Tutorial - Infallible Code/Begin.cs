// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ref : https://www.youtube.com/watch?v=G1bd75R10m4
/// </summary>
public class Begin : State
{
    public Begin(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        BattleSystem.Interface.SetDialogText($"A wild {BattleSystem.Enemy.Name} appeared!");

        yield return new WaitForSeconds(2f);
        
        BattleSystem.SetState(new PlayerTurn(BattleSystem));

    }
}