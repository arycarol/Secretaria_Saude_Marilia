using CareMove.Api.Controller.Base;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Api.Controller.Controller;

public class TransportRequestController : BaseController<ITransportRequestService, InputCreateTransportRequest, InputUpdateTransportRequest, InputGenericDelete, OutputTransportRequest>
{
    public TransportRequestController(ITransportRequestService service) : base(service) { }
}
