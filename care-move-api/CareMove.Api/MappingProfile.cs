using AutoMapper;
using CareMove.Argument.Argument;
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
    }
}