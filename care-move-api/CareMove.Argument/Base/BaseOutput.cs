namespace CareMove.Argument;

public abstract class BaseOutput<TOutput>
    where TOutput : BaseOutput<TOutput>
{
    public virtual long Id { get; protected set; }
    public virtual DateTime CreationDate { get; protected set; }
    public virtual DateTime? ChangeDate { get; protected set; }

    public BaseOutput() { }
}

public class BaseOutput_0 : BaseOutput<BaseOutput_0> { }