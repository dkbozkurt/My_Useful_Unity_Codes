// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

/// <summary>
/// Ref : https://www.youtube.com/watch?v=UoNumkMTx-U
/// </summary>

public abstract class Command
{
    protected IEntity _entity;

    public Command(IEntity entity)
    {
        _entity = entity;
    }

    public abstract void Execute();
    public abstract void Undo();
}