using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Infrastructure.Context;
using CareMove.Infrastructure.Entity.Entity;
using CareMove.Infrastructure.Repository.Base;

namespace CareMove.Infrastructure.Repository.Repository;

public class TransportAssignmentRepository : BaseRepository<TransportAssignment, TransportAssignmentDTO, OutputTransportAssignment>, ITransportAssignmentRepository
{
    public TransportAssignmentRepository(AppDbContext context, IMapper mapper) : base(context, mapper) { }
}