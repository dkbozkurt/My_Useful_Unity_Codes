// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
/// <summary>
/// Ref : https://www.youtube.com/watch?v=G1bd75R10m4
/// </summary>
public class Won : State
{
    public Won(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        BattleSystem.Interface.SetDialogText("You won the battle!");
        yield break;
    }
}