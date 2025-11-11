using CareMove.Api.Controller.Base;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Api.Controller.Controller;

public class TransportAssignmentController : BaseController<ITransportAssignmentService, InputCreateTransportAssignment, InputUpdateTransportAssignment, InputGenericDelete, OutputTransportAssignment>
{
    public TransportAssignmentController(ITransportAssignmentService service) : base(service) { }
}
