namespace CareMove.Argument;

public abstract class BaseInputUpdate<TInputUpdate>
    where TInputUpdate : BaseInputUpdate<TInputUpdate>
{
    public long Id { get; set; }

    public BaseInputUpdate(long id)
    {
        Id = id;
    }
}

public class BaseInputUpdate_0 : BaseInputUpdate<BaseInputUpdate_0>
{
    public BaseInputUpdate_0(long id) : base(id) { }
}