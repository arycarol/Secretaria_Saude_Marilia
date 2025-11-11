using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Domain.Service.Service;

public class VehicleService : BaseService<IVehicleRepository, InputCreateVehicle, InputUpdateVehicle, InputGenericDelete, OutputVehicle, VehicleDTO>, IVehicleService
{
    public VehicleService(IVehicleRepository repository, IMapper mapper) : base(repository, mapper) { }
}