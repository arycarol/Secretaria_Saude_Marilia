using CareMove.Api.Controller.Base;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.Interface.Service.Service;

namespace CareMove.Api.Controller.Controller;

public class UserController : BaseController<IUserService, InputCreateUser, InputUpdateUser, InputGenericDelete, OutputUser>
{
    public UserController(IUserService service) : base(service) { }
}
