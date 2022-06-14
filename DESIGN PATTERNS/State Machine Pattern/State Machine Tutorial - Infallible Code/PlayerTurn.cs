// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using Unity.Collections;
using UnityEngine;
/// <summary>
/// Ref : https://www.youtube.com/watch?v=G1bd75R10m4
/// </summary>
public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
    {
        
    }

    public override IEnumerator Start()
    {
        BattleSystem.Interface.SetDialogText("Choose and action");
        return base.Start();
    }

    public override IEnumerator Attack()
    {
        var isDead = BattleSystem.Enemy.Damage(BattleSystem.Player.Attack);

        yield return new WaitForSeconds(1f);
        
        if(isDead) BattleSystem.SetState(new Won(BattleSystem));
        else BattleSystem.SetState(new EnemyTurn(BattleSystem));
    }

    public override IEnumerator Heal()
    {
        BattleSystem.Interface.SetDialogText($"{BattleSystem.Enemy.Name} feels renewed strength!");

        BattleSystem.Player.Heal(5);
        yield return new WaitForSeconds(1f);
        
        BattleSystem.SetState(new EnemyTurn(BattleSystem));
    }
}