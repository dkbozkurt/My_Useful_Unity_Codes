// Dogukan Kaan Bozkurt
//      github.com/dkbozkurt

/// <summary>
/// Ref : https://www.youtube.com/watch?v=UoNumkMTx-U
/// </summary>

public class ScaleCommand : Command
{
    private readonly float _scaleFactor;

    public ScaleCommand(IEntity entity, float scaleDirection) : base(entity)
    {
        _scaleFactor = scaleDirection == 1f ? 1.1f : 0.9f;
    }

    public override void Execute()
    {
        _entity.transform.localScale *= _scaleFactor;
    }

    public override void Undo()
    {
        _entity.transform.localScale /= _scaleFactor;
    }
}
