// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
/// <summary>
/// Ref : https://www.youtube.com/watch?v=G1bd75R10m4
/// </summary>
public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        if (BattleSystem.Enemy.CurrentHealth <= BattleSystem.Enemy.TotalHealth * 2f)
        {
            BattleSystem.SetState(new Yield(BattleSystem));
            yield break;
        }
        BattleSystem.Interface.SetDialogText($"{BattleSystem.Enemy.Name} attacks!");

        var isDead = BattleSystem.Player.Damage(BattleSystem.Enemy.Attack);

        yield return new WaitForSeconds(1f);
        
        if (isDead) BattleSystem.SetState(new Lost(BattleSystem));
        else BattleSystem.SetState(new PlayerTurn(BattleSystem));
    }
}