using CareMove.Argument;

namespace CareMove.Domain.Interface.Service;

public interface IBaseService<TInputCreate, TInputUpdate, TInputDelete, TOutput>
        where TInputCreate : BaseInputCreate<TInputCreate>
        where TInputUpdate : BaseInputUpdate<TInputUpdate>
        where TInputDelete : BaseInputDelete<TInputDelete>
        where TOutput : BaseOutput<TOutput>
{
    List<TOutput>? GetAll();
    TOutput? Get(long id);
    long Create(TInputCreate? inputCreate);
    List<long>? CreateMultiple(List<TInputCreate>? listInputCreate);
    long Update(TInputUpdate? inputUpdate);
    List<long>? UpdateMultiple(List<TInputUpdate>? listInputUpdate);
    bool Delete(TInputDelete? inputDelete);
    bool DeleteMultiple(List<TInputDelete>? listInputDelete);
}