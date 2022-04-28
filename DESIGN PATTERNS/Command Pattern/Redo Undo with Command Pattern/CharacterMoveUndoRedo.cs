// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Client
///
/// The object that decides which receiver objects it assigns to the command objects, and which commands it assigns to the invoker.
/// 
/// Ref : https://www.youtube.com/watch?v=LRZ1cuXiXTI
/// Ref : https://github.com/onewheelstudio/Programming-Patterns/tree/master/Programming%20Patterns/Assets/Command/Script
/// </summary>

// 3th
public class CharacterMoveUndoRedo : MonoBehaviour
{
   [SerializeField] private List<CommandPatternMove> commandList = new List<CommandPatternMove>();
   
   //
   private int index; 

   #region path drawing

   

   #endregion

   public void AddCommand(ICommand command)
   {
      if(index < commandList.Count)
         commandList.RemoveRange(index, commandList.Count -index);
      
      commandList.Add(command as CommandPatternMove);
      command.Execute();
      index++;
      
      UpdateLine();
   }

   public void UndoCommand()
   {
      if (commandList.Count == 0) return;

      if (index > 0)
      {
         commandList[index - 1].Undo();
         index--;
      }

      UpdateLine();
   }

   public void RedoCommand()
   {
      if (commandList.Count == 0) return;

      if (index < commandList.Count)
      {
         index++;
         commandList[index -1 ].Execute();
      }
      UpdateLine();
   }
   
   public void UpdateLine()
   {
      
   }
}
