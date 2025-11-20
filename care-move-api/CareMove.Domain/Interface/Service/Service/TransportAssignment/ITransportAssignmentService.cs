using CareMove.Argument.Argument;
using CareMove.Argument.Base;

namespace CareMove.Domain.Interface.Service.Service;

public interface ITransportAssignmentService : IBaseService<InputCreateTransportAssignment, InputUpdateTransportAssignment, InputGenericDelete, OutputTransportAssignment>
{
    List<OutputTransportAssignment>? GetListOfToday(long driverUserId);
    List<OutputTransportAssignment>? GetListByDriverId(long driverUserId);
    void Accept(long id);
    void Reject(long id);
}