using System.Text.Json.Serialization;

namespace CareMove.Argument;

public abstract class BaseInputDelete<TInputDelete>
    where TInputDelete : BaseInputDelete<TInputDelete>
{
    public long Id { get; init; }

    public BaseInputDelete() { }

    public BaseInputDelete(long id)
    {
        Id = id;
    }
}

public class BaseInputDelete_0 : BaseInputDelete<BaseInputDelete_0> { }