// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
/// <summary>
/// Ref : https://www.youtube.com/watch?v=G1bd75R10m4
/// </summary>
public class Lost : State
{
    public Lost(BattleSystem battleSystem) : base(battleSystem)
    {
    }
    
    public override IEnumerator Start()
    {
        BattleSystem.Interface.SetDialogText("You were defeated!");
        yield break;
    }
}