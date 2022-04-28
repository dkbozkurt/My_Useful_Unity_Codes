// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Client
/// 
/// Ref : https://www.youtube.com/watch?v=LRZ1cuXiXTI
/// Ref : https://github.com/onewheelstudio/Programming-Patterns/tree/master/Programming%20Patterns/Assets/Command/Script
/// </summary>

// 1st
public class CharacterMoveTurnBased : MonoBehaviour
{
   [SerializeField] private List<CommandPatternMove> commandList = new List<CommandPatternMove>();

   #region path drawing

   

   #endregion

   public void AddCommand(ICommand command)
   {
      commandList.Add(command as CommandPatternMove);
   }

   public void DoMoves()
   {
      StartCoroutine(DoMovesOverTime());
   }

   private IEnumerator DoMovesOverTime()
   {
      foreach (CommandPatternMove move in commandList)
      {
         move.Execute();

         UpdateLine();
         // index++; // Used for path drawing
         yield return new WaitForSeconds(0.5f);
      }
      // index = 0 ;
   }

   public void UpdateLine()
   {
      
   }
}
