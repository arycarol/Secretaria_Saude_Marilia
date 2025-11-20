using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.DTO.DTO.User;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;
using System.Reflection;

namespace CareMove.Domain.Service.Service;

public class TransportRequestService : BaseService<ITransportRequestRepository, InputCreateTransportRequest, InputUpdateTransportRequest, InputGenericDelete, OutputTransportRequest, TransportRequestDTO>, ITransportRequestService
{
    private readonly ITransportAssignmentRepository _transportAssignmentRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUserRepository _userRepository;

    public TransportRequestService(ITransportRequestRepository repository, IMapper mapper, ITransportAssignmentRepository transportAssignmentRepository, IVehicleRepository vehicleRepository, IUserRepository userRepository) : base(repository, mapper)
    {
        _transportAssignmentRepository = transportAssignmentRepository;
        _vehicleRepository = vehicleRepository;
        _userRepository = userRepository;
    }

    public List<OutputTransportRequest>? GetListNonPending()
    {
        return Convert(_repository.GetListNonPending());
    }

    public List<OutputTransportRequest>? GetListPending()
    {
        return Convert(_repository.GetListPending());
    }

    public List<OutputTransportRequest>? GetListByUserId(long userId)
    {
        return Convert(_repository.GetListByUserId(userId));
    }

    public override List<long>? CreateMultiple(List<InputCreateTransportRequest>? listInputCreate)
    {

        List<UserDTO> listUserDTO = _userRepository.GetListByListId((from I in listInputCreate select I.UserId).ToList())!;
        foreach (var i in listInputCreate)
        {
            UserDTO? relatedUserDTO = (from j in listUserDTO where j.Id == i.UserId select j).FirstOrDefault();
            if (relatedUserDTO == null)
                throw new Exception("Id usuário inválido");
        }

        List<TransportRequestDTO> listCreateDTO = new List<TransportRequestDTO>();
        foreach (var i in listInputCreate)
        {
            TransportRequestDTO instance = new TransportRequestDTO();
            foreach (PropertyInfo inputPropertyInfo in typeof(InputCreateTransportRequest).GetProperties())
            {
                PropertyInfo? dtoPropertyInfo = typeof(TransportRequestDTO).GetProperty(inputPropertyInfo.Name);
                if (dtoPropertyInfo != null)
                    dtoPropertyInfo.SetValue(instance, inputPropertyInfo.GetValue(i));
            }
            instance.TransportStatus = "Aguardando";
            listCreateDTO.Add(instance);
        }

        List<long> listId = _repository.CreateMultiple(listCreateDTO);
        return listId;
    }

    public void Accept(InputAcceptTransportRequest inputAcceptTransportRequest)
    {
        TransportRequestDTO? transportRequestDTO = _repository.Get(inputAcceptTransportRequest.Id);
        if (transportRequestDTO == null)
            throw new Exception("Id inválido");

        if (transportRequestDTO.TransportStatus != "Aguardando")
            throw new Exception("Esse registro ja foi processado");

        if (inputAcceptTransportRequest.VehicleId != null)
        {
            VehicleDTO? vehicleDTO = _vehicleRepository.Get(inputAcceptTransportRequest.VehicleId.Value);
            if (vehicleDTO == null)
                throw new Exception("Id do veículo inválido");
        }

        UserDTO? driverUserDTO = _userRepository.Get(inputAcceptTransportRequest.DriverUserId);
        if (driverUserDTO == null)
            throw new Exception("Id do motorista inválido");

        transportRequestDTO.TransportStatus = "Aceito";
        _repository.Update(transportRequestDTO);

        TransportAssignmentDTO transportAssignmentDTO = new TransportAssignmentDTO(inputAcceptTransportRequest.VehicleId, transportRequestDTO.Date, inputAcceptTransportRequest.DriverUserId, transportRequestDTO.UserId, transportRequestDTO.Id, "Aguardando");
        _transportAssignmentRepository.Create(transportAssignmentDTO);
    }

    public void Reject(long id)
    {
        TransportRequestDTO? transportRequestDTO = _repository.Get(id);
        if (transportRequestDTO == null)
            throw new Exception("Id inválido");

        if (transportRequestDTO.TransportStatus != "Aguardando")
            throw new Exception("Esse registro ja foi processado");

        transportRequestDTO.TransportStatus = "Negado";
        _repository.Update(transportRequestDTO);
    }
}