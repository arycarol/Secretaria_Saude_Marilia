using CareMove.Argument;
using CareMove.Domain.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareMove.Api.Controller.Base;

[ApiController]
[Route("api/v1/[controller]")]
public class BaseController<TService, TInputCreate, TInputUpdate, TInputDelete, TOutput> : ControllerBase
    where TService : IBaseService<TInputCreate, TInputUpdate, TInputDelete, TOutput>
    where TInputCreate : BaseInputCreate<TInputCreate>
    where TInputUpdate : BaseInputUpdate<TInputUpdate>
    where TInputDelete : BaseInputDelete<TInputDelete>
    where TOutput : BaseOutput<TOutput>
{

    protected readonly TService _service;

    public BaseController(TService service)
    {
        _service = service;
    }

    #region Create
    [HttpPost]
    public virtual IActionResult Create(TInputCreate inputCreate)
    {
        try
        {
            return Ok(_service.Create(inputCreate));
        }
        catch (ArgumentNullException)
        {
            return BadRequest("A lista está vazia");
        }
        //catch (NotFoundException ex)
        //{
        //    return NotFound(ex.Message);
        //}
        //catch (InvalidArgumentException ex)
        //{
        //    return BadRequest(ex.Message);
        //}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Multiple")]
    public virtual IActionResult CreateMultiple(List<TInputCreate> listInputCreate)
    {
        try
        {
            return Ok(_service.CreateMultiple(listInputCreate));
        }
        catch (ArgumentNullException)
        {
            return BadRequest("A lista está vazia");
        }
        //catch (NotFoundException ex)
        //{
        //    return NotFound(ex.Message);
        //}
        //catch (InvalidArgumentException ex)
        //{
        //    return BadRequest(ex.Message);
        //}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion

    #region Update
    [HttpPut]
    public virtual IActionResult Update(TInputUpdate inputUpdate)
    {
        try
        {
            return Ok(_service.Update(inputUpdate));
        }
        catch (ArgumentNullException)
        {
            return BadRequest("A lista está vazia");
        }
        //catch (NotFoundException ex)
        //{
        //    return NotFound(ex.Message);
        //}
        //catch (InvalidArgumentException ex)
        //{
        //    return BadRequest(ex.Message);
        //}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("Multiple")]
    public virtual IActionResult UpdateMultiple(List<TInputUpdate> listInputUpdate)
    {
        try
        {
            return Ok(_service.UpdateMultiple(listInputUpdate));
        }
        catch (ArgumentNullException)
        {
            return BadRequest("A lista está vazia");
        }
        //catch (NotFoundException ex)
        //{
        //    return NotFound(ex.Message);
        //}
        //catch (InvalidArgumentException ex)
        //{
        //    return BadRequest(ex.Message);
        //}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion

    #region Delete
    [HttpDelete]
    public virtual IActionResult Delete(TInputDelete inputDelete)
    {
        try
        {
            _service.Delete(inputDelete);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest("A lista está vazia");
        }
        //catch (NotFoundException ex)
        //{
        //    return NotFound(ex.Message);
        //}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("Multiple")]
    public virtual IActionResult DeleteMultiple(List<TInputDelete> listInputDelete)
    {
        try
        {
            _service.DeleteMultiple(listInputDelete);
            return Ok();
        }
        catch (ArgumentNullException)
        {
            return BadRequest("A lista está vazia");
        }
        //catch (NotFoundException ex)
        //{
        //    return NotFound(ex.Message);
        //}
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion

    #region Get
    [HttpGet]
    public virtual ActionResult<List<TOutput>> GetAll()
    {
        var query = _service.GetAll();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public virtual ActionResult<TOutput> Get(long id)
    {
        TOutput data = _service.Get(id);

        if (data == null) 
            return NotFound();

        return Ok(data);
    }

    #endregion
}
