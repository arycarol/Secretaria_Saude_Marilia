using AutoMapper;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.DTO.DTO.User;
using CareMove.Domain.Interface.Repository.Repository;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Domain.Service.Service;

public class UserService : BaseService<IUserRepository, InputCreateUser, InputUpdateUser, InputGenericDelete, OutputUser, UserDTO>, IUserService
{
    public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper) { }
}