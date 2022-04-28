// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Client
/// 
/// Ref : https://www.youtube.com/watch?v=LRZ1cuXiXTI
/// Ref : https://github.com/onewheelstudio/Programming-Patterns/tree/master/Programming%20Patterns/Assets/Command/Script
/// </summary>

public class UICommandList : MonoBehaviour
{
    [SerializeField] private List<CommandPatternMove> commandList = new List<CommandPatternMove>();

    [SerializeField] private Text commandText;

    public void AddCommand(ICommand command)
    {
        commandList.Add(command as CommandPatternMove);
        UpdateUIList();
    }

    private void UpdateUIList()
    {
        commandText.text = "Commands: ";

        foreach (CommandPatternMove command in commandList)
        {
            commandText.text += "\n";
            
            Vector3 direction = command.GetMove();

            if (direction.x >= 1) commandText.text += "Right";
            else if (direction.x <= -1) commandText.text += "Left";
            else if (direction.z >= 1) commandText.text += "Up";
            else if (direction.z <= -1) commandText.text += "Down";
        }
    }
        
        
        
}
