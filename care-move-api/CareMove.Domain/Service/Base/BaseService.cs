using AutoMapper;
using CareMove.Argument;
using CareMove.Domain.DTO.Base;
using CareMove.Domain.Interface.Repository;
using CareMove.Domain.Interface.Service;
using System.Reflection;

namespace CareMove.Domain.Service;

public abstract class BaseService<TRepository, TInputCreate, TInputUpdate, TInputDelete, TOutput, TDTO> : IBaseService<TInputCreate, TInputUpdate, TInputDelete, TOutput>
        where TRepository : IBaseRepository<TOutput, TDTO>
        where TInputCreate : BaseInputCreate<TInputCreate>
        where TInputUpdate : BaseInputUpdate<TInputUpdate>
        where TInputDelete : BaseInputDelete<TInputDelete>
        where TOutput : BaseOutput<TOutput>
        where TDTO : BaseDTO<TDTO, TOutput>, new()
{
    protected readonly TRepository _repository;
    protected readonly IMapper _mapper;

    public BaseService(TRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    #region Create
    public long Create(TInputCreate? inputCreate)
    {
        return CreateMultiple([inputCreate])?.FirstOrDefault() ?? 0;
    }

    public List<long>? CreateMultiple(List<TInputCreate>? listInputCreate)
    {
        List<TDTO> listCreateDTO = new List<TDTO>();

        foreach (var i in listInputCreate)
        {
            TDTO instance = new TDTO();
            foreach (PropertyInfo inputPropertyInfo in typeof(TInputCreate).GetProperties())
            {
                PropertyInfo? dtoPropertyInfo = typeof(TDTO).GetProperty(inputPropertyInfo.Name);
                if (dtoPropertyInfo != null)                
                    dtoPropertyInfo.SetValue(instance, inputPropertyInfo.GetValue(i));                
            }
            listCreateDTO.Add(instance);
        }

        List<long> listId = _repository.CreateMultiple(listCreateDTO);
        return listId;
    }
    #endregion

    #region Delete
    public bool Delete(TInputDelete? inputDelete)
    {
        return DeleteMultiple([inputDelete]);
    }

    public bool DeleteMultiple(List<TInputDelete>? listInputDelete)
    {
        List<TDTO> listDelete = _repository.GetListByListId((from i in listInputDelete ?? [] select i.Id).ToList()!)!;
        //if (listDelete == null || listDelete.Count == 0)
        //    throw new NotFoundException("Há um ID inválido na lista de exclusão.");

        _repository.DeleteMultiple(listDelete);

        return true;
    }
    #endregion

    #region Get
    public virtual TOutput? Get(long id)
    {
        return Convert(_repository.Get(id));
    }

    public virtual List<TOutput>? GetAll()
    {
        return Convert(_repository.GetAll());
    }
    #endregion

    #region Update
    public long Update(TInputUpdate? inputUpdate)
    {
        return UpdateMultiple([inputUpdate])?.FirstOrDefault() ?? 0;
    }

    public List<long>? UpdateMultiple(List<TInputUpdate>? listInputUpdate)
    {
        List<TDTO> listOriginalDTO = _repository.GetListByListId((from i in listInputUpdate ?? [] select i.Id).ToList()!)!;

        List<TDTO> listUpdatedDTO = new List<TDTO>();
        foreach (var i in listInputUpdate ?? [])
        {
            TDTO? originalDTO = (from j in listOriginalDTO where j.Id == i.Id select j).FirstOrDefault();
            if (originalDTO == null)
                continue;

            foreach (PropertyInfo inputPropertyInfo in typeof(TInputUpdate).GetProperties())
            {
                PropertyInfo? dtoPropertyInfo = typeof(TDTO).GetProperty(inputPropertyInfo.Name);
                if (dtoPropertyInfo != null)
                    dtoPropertyInfo.SetValue(originalDTO, inputPropertyInfo.GetValue(i));
            }
            listUpdatedDTO.Add(originalDTO);
        }

        List<long> listId = _repository.UpdateMultiple(listUpdatedDTO);
        return listId;
    }
    #endregion

    #region Internal
    #region Internal
    protected TDTO? Convert(TOutput? output)
    {
        return _mapper.Map<TDTO>(output);
    }

    protected List<TDTO>? Convert(List<TOutput>? listOutput)
    {
        return _mapper.Map<List<TDTO>>(listOutput);
    }

    protected TOutput? Convert(TDTO? dto)
    {
        return _mapper.Map<TOutput>(dto);
    }

    protected List<TOutput>? Convert(List<TDTO>? listDTO)
    {
        return _mapper.Map<List<TOutput>>(listDTO);
    }
    #endregion
    #endregion
}