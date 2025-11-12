using CareMove.Argument.Argument;
using CareMove.Argument.Base;

namespace CareMove.Domain.Interface.Service.Service;

public interface IUserService : IBaseService<InputCreateUser, InputUpdateUser, InputGenericDelete, OutputUser>
{
    List<OutputUser>? GetFilterByName(string parameter);
}