using CareMove.Argument.Argument;
using CareMove.Argument.Base;

namespace CareMove.Domain.Interface.Service.Service;

public interface IVehicleService : IBaseService<InputCreateVehicle, InputUpdateVehicle, InputGenericDelete, OutputVehicle> { }