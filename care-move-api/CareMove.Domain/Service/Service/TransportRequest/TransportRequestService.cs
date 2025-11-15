using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;
using System.Reflection;

namespace CareMove.Domain.Service.Service;

public class TransportRequestService : BaseService<ITransportRequestRepository, InputCreateTransportRequest, InputUpdateTransportRequest, InputGenericDelete, OutputTransportRequest, TransportRequestDTO>, ITransportRequestService
{
    private readonly ITransportAssignmentRepository _transportAssignmentRepository;

    public TransportRequestService(ITransportRequestRepository repository, IMapper mapper, ITransportAssignmentRepository transportAssignmentRepository) : base(repository, mapper)
    {
        _transportAssignmentRepository = transportAssignmentRepository;
    }

    public List<OutputTransportRequest>? GetListNonPending()
    {
        return Convert(_repository.GetListNonPending());
    }

    public List<OutputTransportRequest>? GetListPending()
    {
        return Convert(_repository.GetListPending());
    }

    public override List<long>? CreateMultiple(List<InputCreateTransportRequest>? listInputCreate)
    {
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

        transportRequestDTO.TransportStatus = "Negado";
        _repository.Update(transportRequestDTO);
    }
}