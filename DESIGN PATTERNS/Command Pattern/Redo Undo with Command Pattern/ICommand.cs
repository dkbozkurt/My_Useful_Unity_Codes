// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

/// <summary>
/// Command
///
/// An object that knows about the receiver and invokes a method of the receiver
/// Values for the receiver method are stored on the command object
/// 
/// Ref : https://www.youtube.com/watch?v=LRZ1cuXiXTI
/// Ref : https://github.com/onewheelstudio/Programming-Patterns/tree/master/Programming%20Patterns/Assets/Command/Script
/// </summary>
public interface ICommand
{
    // This will contain all the code for the command to complete its action
    void Execute();

    // Reverses whatever happened in the execute function
    void Undo();
}
