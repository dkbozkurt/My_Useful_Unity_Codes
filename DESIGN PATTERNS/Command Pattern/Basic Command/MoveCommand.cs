// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=UoNumkMTx-U
/// </summary>

public class MoveCommand : Command
{
    private Vector3 _direction;

    public MoveCommand(IEntity entity, Vector3 direction) : base(entity)
    {
        _direction = direction;
    }

    public override void Execute()
    {
        _entity.transform.position += _direction * 0.1f;
    }

    public override void Undo()
    {
        _entity.transform.position -= _direction * 0.1f;
    }
}
