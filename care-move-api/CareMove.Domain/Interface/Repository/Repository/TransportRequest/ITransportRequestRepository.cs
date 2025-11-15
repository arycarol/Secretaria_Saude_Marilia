using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO;

namespace CareMove.Domain.Interface.Repository.Repository;

public interface ITransportRequestRepository : IBaseRepository<OutputTransportRequest, TransportRequestDTO>
{
    List<TransportRequestDTO>? GetListPending();
    List<TransportRequestDTO>? GetListNonPending();
}