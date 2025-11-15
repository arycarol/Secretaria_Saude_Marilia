using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Infrastructure.Context;
using CareMove.Infrastructure.Entity.Entity;
using CareMove.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace CareMove.Infrastructure.Repository.Repository;

public class TransportAssignmentRepository : BaseRepository<TransportAssignment, TransportAssignmentDTO, OutputTransportAssignment>, ITransportAssignmentRepository
{
    public TransportAssignmentRepository(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public List<TransportAssignmentDTO>? GetListOfToday()
    {
        DateOnly dataAtual = DateOnly.FromDateTime(DateTime.Today);

        return Convert(_dbset.Where(x => x.Date == dataAtual).AsNoTracking().ToList());
    }
}