using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Infrastructure.Context;
using CareMove.Infrastructure.Entity.Entity;
using CareMove.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace CareMove.Infrastructure.Repository.Repository;

public class TransportRequestRepository : BaseRepository<TransportRequest, TransportRequestDTO, OutputTransportRequest>, ITransportRequestRepository
{
    public TransportRequestRepository(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public List<TransportRequestDTO>? GetListPending()
    {
        return Convert(_dbset.Where(x => x.TransportStatus == "Aguardando").AsNoTracking().ToList());
    }

    public List<TransportRequestDTO>? GetListNonPending()
    {
        return Convert(_dbset.Where(x => x.TransportStatus != "Aguardando").AsNoTracking().ToList());
    }
}