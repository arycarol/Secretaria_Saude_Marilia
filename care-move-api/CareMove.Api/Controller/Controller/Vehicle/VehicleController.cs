using CareMove.Api.Controller.Base;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Api.Controller.Controller;

public class VehicleController : BaseController<IVehicleService, InputCreateVehicle, InputUpdateVehicle, InputGenericDelete, OutputVehicle>
{
    public VehicleController(IVehicleService service) : base(service) { }
}
