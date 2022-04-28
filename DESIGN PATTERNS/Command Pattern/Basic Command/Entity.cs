// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// Ref : https://www.youtube.com/watch?v=UoNumkMTx-U
/// </summary>

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(CommandProcessor))]
public class Entity : MonoBehaviour, IEntity
{
    private InputReader _inputReader;
    private CommandProcessor _commandProcessor;
    
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _commandProcessor = GetComponent<CommandProcessor>();
    }
    
    void Update()
    {
        ExecuteOperations();    
    }

    private void ExecuteOperations()
    {
        var direction = _inputReader.ReadInput();
        if (direction != Vector3.zero)
        {
            var moveCommand = new MoveCommand(this, direction);
            _commandProcessor.ExecuteCommand(moveCommand);
        }
        
        //
        if (_inputReader.ReadUndo())
        {
            _commandProcessor.Undo();
        }

        float scaleDirection = _inputReader.ReadScale();

        if (scaleDirection != 0f)
        {
            _commandProcessor.ExecuteCommand(new ScaleCommand(this, scaleDirection));
        }
    }

    public void MoveFromTo(Vector3 startPosition, Vector3 endPosition)
    {
        throw new System.NotImplementedException();
    }
}
