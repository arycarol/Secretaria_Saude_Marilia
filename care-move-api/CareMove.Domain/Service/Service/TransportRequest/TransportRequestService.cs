using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Domain.Service.Service;

public class TransportRequestService : BaseService<ITransportRequestRepository, InputCreateTransportRequest, InputUpdateTransportRequest, InputGenericDelete, OutputTransportRequest, TransportRequestDTO>, ITransportRequestService
{
    public TransportRequestService(ITransportRequestRepository repository, IMapper mapper) : base(repository, mapper) { }
}