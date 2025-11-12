using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO;
using CareMove.Domain.DTO.DTO.User;
using CareMove.Infrastructure.Entity.Entity;

namespace CareMove.Domain;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDTO>();
        CreateMap<UserDTO, OutputUser>();
        CreateMap<UserDTO, User>();
        CreateMap<OutputUser, UserDTO>();

        // TransportAssignment
        CreateMap<TransportAssignment, TransportAssignmentDTO>();
        CreateMap<TransportAssignmentDTO, OutputTransportAssignment>();
        CreateMap<TransportAssignmentDTO, TransportAssignment>();
        CreateMap<OutputTransportAssignment, TransportAssignmentDTO>();

        // TransportRequest
        CreateMap<TransportRequest, TransportRequestDTO>();
        CreateMap<TransportRequestDTO, OutputTransportRequest>();
        CreateMap<TransportRequestDTO, TransportRequest>();
        CreateMap<OutputTransportRequest, TransportRequestDTO>();

        // Vehicle
        CreateMap<Vehicle, VehicleDTO>();
        CreateMap<VehicleDTO, OutputVehicle>();
        CreateMap<VehicleDTO, Vehicle>();
        CreateMap<OutputVehicle, VehicleDTO>();
    }
}