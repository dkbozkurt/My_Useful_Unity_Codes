// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// Ref : https://www.youtube.com/watch?v=UoNumkMTx-U
/// </summary>

public interface IEntity
{
    Transform transform { get; }
    void MoveFromTo(Vector3 startPosition, Vector3 endPosition);
}
