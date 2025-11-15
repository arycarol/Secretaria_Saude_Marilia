using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO;

namespace CareMove.Domain.Interface.Repository.Repository;

public interface ITransportAssignmentRepository : IBaseRepository<OutputTransportAssignment, TransportAssignmentDTO>
{
    List<TransportAssignmentDTO>? GetListOfToday();
}