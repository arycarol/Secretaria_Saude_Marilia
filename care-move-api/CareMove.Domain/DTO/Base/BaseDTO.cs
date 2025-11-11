using CareMove.Argument;

namespace CareMove.Domain.DTO.Base;

public class BaseDTO<TDTO, TOutput>
    where TDTO: BaseDTO<TDTO, TOutput>
    where TOutput : BaseOutput<TOutput>
{
    public virtual long Id { get; protected set; }
    public virtual DateTime CreationDate { get; protected set; }
    public virtual DateTime? ChangeDate { get; protected set; }

    public BaseDTO() { }
}