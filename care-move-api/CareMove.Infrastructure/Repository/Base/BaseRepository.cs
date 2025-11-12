using AutoMapper;
using CareMove.Argument;
using CareMove.Domain.DTO.Base;
using CareMove.Domain.Interface.Repository;
using CareMove.Infrastructure.Context;
using CareMove.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace CareMove.Infrastructure.Repository.Base;

public abstract class BaseRepository<TEntity, TDTO, TOutput> : IBaseRepository<TOutput, TDTO>
        where TEntity : BaseEntity<TEntity, TDTO, TOutput>
        where TDTO : BaseDTO<TDTO, TOutput>
        where TOutput : BaseOutput<TOutput>
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbset;
    protected readonly IMapper _mapper;

    public BaseRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _dbset = _context.Set<TEntity>();
        _mapper = mapper;
    }

    #region Create
    public virtual long Create(TDTO? entity)
    {
        return CreateMultiple([entity]).First();
    }

    public virtual List<long> CreateMultiple(List<TDTO>? listDTO)
    {
        if (listDTO == null || listDTO.Count == 0)
            return [];

        var listEntity = Convert(listDTO);
        _context.AddRange(from i in listEntity select i.SetCreationDate());
        _context.SaveChanges();

        return (from i in listEntity select i.Id).ToList();
    }
    #endregion

    #region Get
    public virtual List<TDTO>? GetAll()
    {
        return Convert(_dbset.ToList());
    }

    public virtual List<TDTO>? GetListByListId(List<long> listId)
    {
        if (listId == null || listId.Count == 0)
            return [];

        return Convert(_dbset.Where(x => listId.Contains(x.Id)).AsNoTracking().ToList());
    }

    public virtual TDTO? Get(long id)
    {
        return Convert(_dbset.Where(x => x.Id == id).AsNoTracking().FirstOrDefault());
    }
    #endregion

    #region Update
    public virtual long Update(TDTO? updateDTO)
    {
        return UpdateMultiple([updateDTO]).FirstOrDefault();
    }

    public List<long> UpdateMultiple(List<TDTO>? listUpdateDTO)
    {
        if (listUpdateDTO == null || listUpdateDTO.Count == 0)
            return [];

        List<TEntity> listEntity = Convert(listUpdateDTO)!;
        _context.UpdateRange(from i in listEntity select i.SetChangeDate());
        _context.SaveChanges();

        return (from i in listEntity select i.Id).ToList();
    }
    #endregion

    #region Delete
    public virtual void Delete(TDTO? dto)
    {
        DeleteMultiple([dto]);
    }

    public void DeleteMultiple(List<TDTO>? listDTO)
    {
        if (listDTO == null || listDTO.Count == 0)
            return;

        _context.RemoveRange(Convert(listDTO)!);
        _context.SaveChanges();
    }
    #endregion

    #region Internal
    protected TDTO? Convert(TEntity? entity)
    {
        return _mapper.Map<TDTO>(entity);
    }

    protected List<TDTO>? Convert(List<TEntity>? listEntity)
    {
        return _mapper.Map<List<TDTO>>(listEntity);
    }

    protected TEntity? Convert(TDTO? dto)
    {
        return _mapper.Map<TEntity>(dto);
    }

    protected List<TEntity>? Convert(List<TDTO>? listDTO)
    {
        return _mapper.Map<List<TEntity>>(listDTO);
    }
    #endregion
}