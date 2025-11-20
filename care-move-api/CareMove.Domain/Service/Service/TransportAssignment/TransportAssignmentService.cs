using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Domain.Service.Service;

public class TransportAssignmentService : BaseService<ITransportAssignmentRepository, InputCreateTransportAssignment, InputUpdateTransportAssignment, InputGenericDelete, OutputTransportAssignment, TransportAssignmentDTO>, ITransportAssignmentService
{
    public TransportAssignmentService(ITransportAssignmentRepository repository, IMapper mapper) : base(repository, mapper) { }

    public List<OutputTransportAssignment>? GetListOfToday(long driverUserId)
    {
        return Convert(_repository.GetListOfToday(driverUserId));
    }

    public List<OutputTransportAssignment>? GetListByDriverId(long driverUserId)
    {
        return Convert(_repository.GetListByDriverId(driverUserId));
    }

    public void Accept(long id)
    {
        TransportAssignmentDTO? transportAssignmentDTO = _repository.Get(id);
        if (transportAssignmentDTO == null)
            throw new Exception("Id inválido");

        transportAssignmentDTO.TransportAssignmentStatus = "Concluido";
        _repository.Update(transportAssignmentDTO);
    }

    public void Reject(long id)
    {
        TransportAssignmentDTO? transportAssignmentDTO = _repository.Get(id);
        if (transportAssignmentDTO == null)
            throw new Exception("Id inválido");

        transportAssignmentDTO.TransportAssignmentStatus = "Negado pelo motorista";
        _repository.Update(transportAssignmentDTO);
    }
}