using CareMove.Api.Controller.Base;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.Interface.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace CareMove.Api.Controller.Controller;

public class TransportAssignmentController : BaseController<ITransportAssignmentService, InputCreateTransportAssignment, InputUpdateTransportAssignment, InputGenericDelete, OutputTransportAssignment>
{
    public TransportAssignmentController(ITransportAssignmentService service) : base(service) { }

    [HttpGet("GetListOfToday")]
    public ActionResult<List<OutputTransportAssignment>> GetListOfToday()
    {
        var query = _service.GetListOfToday();
        return Ok(query);
    }

    [HttpPost("Accept/{id}")]
    public virtual IActionResult Accept(long id)
    {
        try
        {
            _service.Accept(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Reject/{id}")]
    public virtual IActionResult Reject(long id)
    {
        try
        {
            _service.Reject(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
