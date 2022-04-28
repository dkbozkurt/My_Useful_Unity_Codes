// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

using UnityEngine;

/// <summary>
/// Receiver
///
/// The object that does the work when the execute method is called on the command object.
/// 
/// Ref : https://www.youtube.com/watch?v=LRZ1cuXiXTI
/// Ref : https://github.com/onewheelstudio/Programming-Patterns/tree/master/Programming%20Patterns/Assets/Command/Script
/// </summary>

// Move
[System.Serializable]
public class CommandPatternMove : ICommand
{
    [SerializeField]
    private Vector3 direction = Vector3.zero;
    private float distance;
    private Transform objectToMove;

    public CommandPatternMove(Transform objectToMove, Vector3 direction, float distance = 1f)
    {
        this.direction = direction;
        this.objectToMove = objectToMove;
        this.distance = distance;
    }

    public void Execute()
    {
        objectToMove.position += direction * distance;
    }

    public void Undo()
    {
        objectToMove.position -= direction * distance;
    }

    // used to draw the path of the object
    public Vector3 GetMove()
    {
        return direction * distance;
    }

}
