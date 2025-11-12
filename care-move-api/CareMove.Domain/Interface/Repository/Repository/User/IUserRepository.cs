using CareMove.Argument.Argument;
using CareMove.Domain.DTO.DTO.User;

namespace CareMove.Domain.Interface.Repository.Repository;

public interface IUserRepository : IBaseRepository<OutputUser, UserDTO>
{
    List<UserDTO>? GetFilterByName(string parameter);
}