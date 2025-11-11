using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Domain.Service.Service;

public class TransportAssignmentService : BaseService<ITransportAssignmentRepository, InputCreateTransportAssignment, InputUpdateTransportAssignment, InputGenericDelete, OutputTransportAssignment, TransportAssignmentDTO>, ITransportAssignmentService
{
    public TransportAssignmentService(ITransportAssignmentRepository repository, IMapper mapper) : base(repository, mapper) { }
}