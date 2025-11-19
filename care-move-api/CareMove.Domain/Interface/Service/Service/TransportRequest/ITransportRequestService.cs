using CareMove.Argument.Argument;
using CareMove.Argument.Base;

namespace CareMove.Domain.Interface.Service.Service;

public interface ITransportRequestService : IBaseService<InputCreateTransportRequest, InputUpdateTransportRequest, InputGenericDelete, OutputTransportRequest>
{
    List<OutputTransportRequest>? GetListPending();
    List<OutputTransportRequest>? GetListNonPending();
    List<OutputTransportRequest>? GetListByUserId(long userId);
    void Accept(InputAcceptTransportRequest inputAcceptTransportRequest);
    void Reject(long id);
}