using CareMove.Argument;
using CareMove.Domain.DTO.Base;

namespace CareMove.Domain.Interface.Repository;

public interface IBaseRepository<TOutput, TDTO>
        where TDTO : BaseDTO<TDTO, TOutput>
        where TOutput : BaseOutput<TOutput>
{
    List<TDTO>? GetAll();
    TDTO? Get(long id);
    List<TDTO>? GetListByListId(List<long> listId);
    long Create(TDTO? dto);
    List<long> CreateMultiple(List<TDTO>? listDTO);
    long Update(TDTO? dto);
    List<long> UpdateMultiple(List<TDTO>? listDTO);
    void Delete(TDTO? dto);
    void DeleteMultiple(List<TDTO>? dto);
}