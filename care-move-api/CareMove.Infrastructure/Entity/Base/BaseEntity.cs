using CareMove.Argument;
using CareMove.Domain.DTO.Base;

namespace CareMove.Infrastructure.Entity;

public abstract class BaseEntity<TEntity, TDTO, TOutput>
    where TEntity : BaseEntity<TEntity, TDTO, TOutput>
    where TDTO : BaseDTO<TDTO, TOutput>
    where TOutput : BaseOutput<TOutput>
{
    public long Id { get; protected set; }
    public virtual DateTime CreationDate { get; protected set; }
    public virtual DateTime? ChangeDate { get; protected set; }

    public TEntity SetCreationDate()
    {
        CreationDate = DateTime.Now;
        return (TEntity)this;
    }

    public TEntity SetChangeDate()
    {
        ChangeDate = DateTime.Now;
        return (TEntity)this;
    }
}