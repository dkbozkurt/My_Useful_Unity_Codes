// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Client
/// 
/// Ref : https://www.youtube.com/watch?v=LRZ1cuXiXTI
/// Ref : https://github.com/onewheelstudio/Programming-Patterns/tree/master/Programming%20Patterns/Assets/Command/Script
/// </summary>

// 2nd
public class CharacterMoveUndo : MonoBehaviour
{
   [SerializeField] private List<CommandPatternMove> commandList = new List<CommandPatternMove>();

   #region path drawing

   

   #endregion

   public void AddCommand(ICommand command)
   {
      commandList.Add(command as CommandPatternMove);
      command.Execute();
      
      UpdateLine();
      
   }

   public void UndoCommand()
   {
      if (commandList.Count == 0) return;

      commandList[commandList.Count - 1].Undo();
      commandList.RemoveAt(commandList.Count -1 );
      
      UpdateLine();
   }

   public void UpdateLine()
   {
      
   }
}
