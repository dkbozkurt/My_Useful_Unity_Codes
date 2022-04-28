// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Invoker
///
/// The object that knows how to execute a command and can optionally do bookkeeping about the command execution.
/// The invoker doesn't know about a concrete command.
/// 
/// Ref : https://www.youtube.com/watch?v=LRZ1cuXiXTI
/// Ref : https://github.com/onewheelstudio/Programming-Patterns/tree/master/Programming%20Patterns/Assets/Command/Script
/// </summary>

public class InputManagerTurnBased : MonoBehaviour
{
    [SerializeField] private Button up;
    [SerializeField] private Button down;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    
    [SerializeField] private Button doTurn;
    
    [SerializeField] private CharacterMoveTurnBased character;
    
    [SerializeField] private UICommandList uiCommandList;
    
    //
    [SerializeField] private Button undo;
    
    [SerializeField] private CharacterMoveUndo characterUndo;
    //
    
    
    //
    [SerializeField] private Button undo2;
    [SerializeField] private Button redo;
    
    [SerializeField] private CharacterMoveUndoRedo characterMoveUndoRedo;
    //

    private void OnEnable()
    {
        up?.onClick.AddListener(() => SendMoveCommand(character.transform,Vector3.forward,1f));
        down?.onClick.AddListener(() => SendMoveCommand(character.transform, Vector3.back,1f));
        left?.onClick.AddListener(() => SendMoveCommand(character.transform, Vector3.left,1f));
        right?.onClick.AddListener(() => SendMoveCommand(character.transform, Vector3.right,1f));
        
        doTurn?.onClick.AddListener(() => character.DoMoves());
        
        // Addition for CharacterMoveUndo
        undo?.onClick.AddListener(() => characterUndo?.UndoCommand());
        //
        
        // Addition for CharacterMoveUndoRedo
        undo2?.onClick.AddListener(() => characterMoveUndoRedo?.UndoCommand());
        redo?.onClick.AddListener(() => characterMoveUndoRedo?.RedoCommand());
        //
    }

    private void SendMoveCommand(Transform objectToMove,Vector3 direction, float distance)
    {
        ICommand movement = new CommandPatternMove(objectToMove, direction, distance);
        character?.AddCommand(movement);
        characterUndo?.AddCommand(movement);
        characterMoveUndoRedo?.AddCommand(movement);
        uiCommandList?.AddCommand(movement);
    }
}
