using CareMove.Api.Controller.Base;
using CareMove.Argument.Argument;
using CareMove.Argument.Base;
using CareMove.Domain.Interface.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace CareMove.Api.Controller.Controller;

public class TransportRequestController : BaseController<ITransportRequestService, InputCreateTransportRequest, InputUpdateTransportRequest, InputGenericDelete, OutputTransportRequest>
{
    public TransportRequestController(ITransportRequestService service) : base(service) { }

    [HttpGet("GetListPending")]
    public ActionResult<List<OutputTransportRequest>> GetListPending()
    {
        var query = _service.GetListPending();
        return Ok(query);
    }

    [HttpGet("GetListNonPending")]
    public ActionResult<List<OutputTransportRequest>> GetListNonPending()
    {
        var query = _service.GetListNonPending();
        return Ok(query);
    }

    [HttpGet("GetListByUserId/{id}")]
    public ActionResult<List<OutputTransportRequest>> GetListByUserId(long id)
    {
        var query = _service.GetListByUserId(id);
        return Ok(query);
    }

    [HttpPost("Accept")]
    public virtual IActionResult Accept(InputAcceptTransportRequest inputAcceptTransportRequest)
    {
        try
        {
            _service.Accept(inputAcceptTransportRequest);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Reject")]
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
