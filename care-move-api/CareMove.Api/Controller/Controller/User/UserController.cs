using CareMove.Api.Controller.Base;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.Interface.Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareMove.Api.Controller.Controller;

public class UserController : BaseController<IUserService, InputCreateUser, InputUpdateUser, InputGenericDelete, OutputUser>
{
    public UserController(IUserService service) : base(service) { }

    [HttpGet("GetFilterByName/{parameter}")]
    public virtual ActionResult<OutputUser> GetFilterByName(string parameter)
    {
        List<OutputUser>? data = _service.GetFilterByName(parameter);

        if (data == null)
            return NotFound();

        return Ok(data);
    }

    [AllowAnonymous]
    [HttpPost]
    public override IActionResult Create(InputCreateUser inputCreate)
    {
        return base.Create(inputCreate);
    }
}
